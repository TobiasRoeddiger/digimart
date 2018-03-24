using System;
using System.Collections.Generic;

public class Form
{
    public IDictionary<string, FormEntry> Entries { get; private set; }

    public Form()
    {
        Entries = new Dictionary<string, FormEntry>();
    }

    public void Add(FormEntry entry, string name = null)
    {
        name = name ?? (new Random()).Next(100000, 10000000).ToString("X");
        Entries.Add(name, entry);
    }

    public ICollection<FormEntry> GetEntries()
    {
        return Entries.Values;
    }

    public FormEntry GetEntry(string name)
    {
        return Entries.ContainsKey(name) ? Entries[name] : null;
    }
}
