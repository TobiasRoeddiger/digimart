using System.ComponentModel;

public partial class Product
{

    public string Id { get; set; }
    public string ImageName { get; set; }

    [DisplayName("Brand")]
    public string CompanyName { get; set; }
    [DisplayName("Product")]
    public string ProductName { get; set; }
    [DisplayName("Type")]
    public string ProductSubType { get; set; }

    public double Price { get; set; }
}
