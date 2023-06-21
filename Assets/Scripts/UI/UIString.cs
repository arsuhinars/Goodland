using System;

public class UIString : AbstractReactiveString
{
    protected override IReactiveDictionary<string, string> GetDefaultSource()
    {
        return LocalizationManager.Instance.LocalizationTable;
    }

    protected override IReactiveDictionary<string, string> GetSource(string name)
    {
        return name switch
        {
            "t" or "tranlsate" => LocalizationManager.Instance.LocalizationTable,
            "s" or "state" => UIStateManager.Instance.State,
            _ => throw new ArgumentException("Unknown reactive source name"),
        };
    }
}
