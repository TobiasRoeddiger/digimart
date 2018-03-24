class AdModule : IDigiModule
{
    public IDigiFilter Filter { get; private set; }
    public Form Form { get; private set; }


    public AdModule()
    {
        Filter = new AdFilter();
        FillForm();
    }


    public void ApplyForm()
    {
    }

    
    private void FillForm()
    {
        Form = new Form();
        Form.Add(new FormEntry(FormEntryType.Button, "Save"), "SaveButton");
        Form.Add(new FormEntry(FormEntryType.Button, "Cancel"), "CancelButton");
    }
}
