using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ReactiveElement : VisualElement
{
    public new class UxmlFactory : UxmlFactory<ReactiveElement, UxmlTraits> { }

    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlStringAttributeDescription m_TemplateText = new() { name = "template-text" };

        public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
        {
            get
            {
                yield return new UxmlChildElementDescription(typeof(TextElement));
                yield return new UxmlChildElementDescription(typeof(Label));
                yield return new UxmlChildElementDescription(typeof(Button));
            }
        }

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);

            (ve as ReactiveElement).TemplateText = m_TemplateText.GetValueFromBag(bag, cc);
        }
    }

    private TextElement m_textElement;
    private UIString m_uiString;
    private string m_templateText;
    private bool m_isDirty = false;
    private bool m_isVisible = false;

    public string TemplateText
    {
        get => m_templateText;
        set
        {
            m_templateText = value;
            if (Application.isPlaying)
            {
                m_uiString.Template = value;
            }
        }
    }

    public ReactiveElement()
    {
        if (Application.isPlaying)
        {
            m_uiString = new();
            m_uiString.OnUpdate += OnStringUpdate;
        }

        RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
        RegisterCallback<AttachToPanelEvent>(OnAttachedToPanel);
    }

    private void OnAttachedToPanel(AttachToPanelEvent ev)
    {
        var childrenEnumerator = Children().GetEnumerator();
        childrenEnumerator.MoveNext();

        m_textElement = childrenEnumerator.Current as TextElement;
    }

    private void OnStringUpdate()
    {
        if (!m_isDirty)
        {
            m_isDirty = true;
            UIManager.Instance.DoOnEndOfFrame(UpdateTextElement);
        }
    }

    private void OnGeometryChange(GeometryChangedEvent ev)
    {
        m_isVisible = ev.newRect.width > 0f && ev.newRect.height > 0f;
        UpdateTextElement();
    }

    private void UpdateTextElement()
    {
        if (m_isDirty && m_isVisible)
        {
            m_textElement.text = m_uiString.Value;
            m_isDirty = false;
        }
    }
}
