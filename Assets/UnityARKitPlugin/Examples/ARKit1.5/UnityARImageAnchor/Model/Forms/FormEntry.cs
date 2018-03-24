using System.Collections.Generic;

public class FormEntry
{
    public FormEntryType Type { get; protected set; }
    public string Label { get; protected set; }
    public string Value { get; set; }

    public FormEntry(FormEntryType type, string label = "")
    {
        Type = type;
        Label = label;
    }
}

public class FormSlider : FormEntry
{
    public double Min { get; private set; }
    public double Max { get; private set; }

    public FormSlider(double min, double max) : base(FormEntryType.Slider)
    {
        Min = min;
        Max = max;
    }
}

public class Select : FormEntry
{
    public IDictionary<string, string> Dictionary { get; private set; }

    public Select() : base(FormEntryType.Select)
    {
        Dictionary = new Dictionary<string, string>();
    }

    public void Add(string key, string value)
    {
        Dictionary.Add(key, value);
    }
}
