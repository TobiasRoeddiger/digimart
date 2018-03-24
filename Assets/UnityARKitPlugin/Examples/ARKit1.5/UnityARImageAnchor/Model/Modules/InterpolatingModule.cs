public class InterpolatingModule : IDigiModule
{

    public IDigiFilter Filter { get; private set; }
    public Form Form { get; private set; }

    public InterpolatingModule()
    {
        CreateFilterForProperty("Price");
        FillForm();
    }


    public void ApplyForm()
    {
        var field = Form.GetEntry("PropertySelect").Value;
        CreateFilterForProperty(field);
    }


    private void CreateFilterForProperty(string field)
    {
        var range = Product.Ranges[field];
        Filter = new InterpolatingFilter(Product.GetPropertySelector(field), range.Min, range.Max);

    }

    private void FillForm()
    {
        Form = new Form();
        Form.Add(new FormEntry(FormEntryType.Label, "Value"));

        var select = new Select();
        select.Add("Calories", "Calories");
        select.Add("Fat", "Fat");
        select.Add("Sugar", "Sugar");
        select.Add("Salt", "Salt");
        select.Add("Price", "Price");
        Form.Add(select, "PropertySelect");
    }
}
