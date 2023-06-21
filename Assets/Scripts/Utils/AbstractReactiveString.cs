using System;
using System.Text;
using System.Text.RegularExpressions;
using ReactDict = IReactiveDictionary<string, string>;

public abstract class AbstractReactiveString
{
    public event Action OnUpdate;

    public string Value
    {
        get
        {
            if (m_isDirty)
            {
                BuildString();
            }

            return m_strBuilder.ToString();
        }
    }

    public string Template
    {
        get => m_template;
        set
        {
            m_template = value;
            ParseTemplate();
        }
    }

    private readonly Regex tagsParser = new(@"{{\s*(([a-zA-Z0-9_-]+)\.)?([a-zA-Z0-9_-]+)\s*}}");

    private StringBuilder m_strBuilder = new();
    private bool m_isDirty = true;
    private string m_template;
    private ParsedTag[] m_parsedTags = new ParsedTag[0];

    private struct ParsedTag
    {
        public int startIdx;
        public int length;

        public ReactDict source;
        public string sourceKey;
    }

    ~AbstractReactiveString()
    {
        RemoveTagsHandlers();
    }

    protected abstract ReactDict GetDefaultSource();

    protected abstract ReactDict GetSource(string name);

    private void ParseTemplate()
    {
        RemoveTagsHandlers();

        var matches = tagsParser.Matches(m_template);

        m_parsedTags = new ParsedTag[matches.Count];

        int i = 0;
        foreach (Match match in matches)
        {
            var sourceGroup = match.Groups[2];
            var keyGroup = match.Groups[3];

            m_parsedTags[i].startIdx = match.Index;
            m_parsedTags[i].length = match.Length;
            m_parsedTags[i].source = sourceGroup.Success ? GetSource(sourceGroup.Value) : GetDefaultSource();
            m_parsedTags[i].sourceKey = keyGroup.Value;

            m_parsedTags[i].source.AddHandler(m_parsedTags[i].sourceKey, ItemUpdateHandler);

            ++i;
        }

        Array.Sort(m_parsedTags, (left, right) => left.startIdx - right.startIdx);

        m_isDirty = true;
        OnUpdate?.Invoke();
    }

    private void BuildString()
    {
        m_strBuilder.Clear();

        if (m_parsedTags.Length == 0)
        {
            m_strBuilder.Append(m_template);
            return;
        }

        m_strBuilder.Append(m_template, 0, m_parsedTags[0].startIdx);

        for (int i = 0; i < m_parsedTags.Length; ++i)
        {
            var t = m_parsedTags[i];

            if (i > 0)
            {
                var lastT = m_parsedTags[i - 1];
                m_strBuilder.Append(
                    m_template,
                    lastT.startIdx + lastT.length,
                    t.startIdx - lastT.startIdx - lastT.length
                );
            }

            t.source.TryGetValue(t.sourceKey, out var value);
            m_strBuilder.Append(value);
        }

        var lastTag = m_parsedTags[m_parsedTags.Length - 1];
        int lastIdx = lastTag.startIdx + lastTag.length;
        m_strBuilder.Append(m_template, lastIdx, m_template.Length - lastIdx);

        m_isDirty = false;
    }

    private void ItemUpdateHandler(string key, object oldValue, object newValue)
    {
        m_isDirty = true;
        OnUpdate?.Invoke();
    }

    private void RemoveTagsHandlers()
    {
        foreach (var tag in m_parsedTags)
        {
            tag.source.RemoveHandler(tag.sourceKey, ItemUpdateHandler);
        }
    }
}
