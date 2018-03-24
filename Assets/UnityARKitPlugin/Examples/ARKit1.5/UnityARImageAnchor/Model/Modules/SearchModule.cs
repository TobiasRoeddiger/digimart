class SearchModule : IDigiModule
{
    public IDigiFilter Filter { get; private set; }
    public Form Form { get; private set; }


    public SearchModule()
    {
        CreateFilterForSearchTerm("----");
        FillForm();
    }


    public void ApplyForm()
    {
        var value = Form.GetEntry("Search").Value;
        CreateFilterForSearchTerm(value);
    }


    private void CreateFilterForSearchTerm(string value)
    {
        Filter = new SearchFilter(value);
    }

    private void FillForm()
    {
        Form = new Form();
        Form.Add(new FormEntry(FormEntryType.Label, "Search Terms"), "SearchLabel");
        Form.Add(new FormEntry(FormEntryType.TextField), "Search");

        Form.Add(new FormEntry(FormEntryType.Line));

        Form.Add(new FormEntry(FormEntryType.Button, "Save"), "SaveButton");
        Form.Add(new FormEntry(FormEntryType.Button, "Cancel"), "CancelButton");
    }
}
