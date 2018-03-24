﻿using System;
using System.Collections.Generic;

class AllergyModule : IDigiModule
{
    public IDigiFilter Filter { get; private set; }
    public Form Form { get; private set; }

    private static readonly IDictionary<string, Func<Product, bool>> _SELECTORS = new Dictionary<string, Func<Product, bool>>()
    {
        { "Milk", p => p.ContainsMilk },
        { "Nuts", p => p.ContainsNuts }
    };


    public AllergyModule()
    {
        CreateFilterForSelectors(new List<Func<Product, bool>>());
        FillForm();
    }


    public void ApplyForm()
    {
        IList<Func<Product, bool>> list = new List<Func<Product, bool>>();
        foreach (var pair in _SELECTORS)
        {
            var value = Form.GetEntry(pair.Key).Value;
            if (bool.Parse(value))
                list.Add(pair.Value);
        }

        CreateFilterForSelectors(list);
    }


    private void CreateFilterForSelectors(IList<Func<Product, bool>> selectors)
    {
        Filter = new AllergyFilter(selectors);
    }

    private void FillForm()
    {
        Form = new Form();
        Form.Add(new FormEntry(FormEntryType.Label, "Allergies"), "AllergyLabel");
        Form.Add(new FormEntry(FormEntryType.Checkbox, "Milk"), "Milk");
        Form.Add(new FormEntry(FormEntryType.Checkbox, "Nuts"), "Nuts");

        Form.Add(new FormEntry(FormEntryType.Line));

        Form.Add(new FormEntry(FormEntryType.Button, "Save"), "SaveButton");
        Form.Add(new FormEntry(FormEntryType.Button, "Cancel"), "CancelButton");
    }
}
