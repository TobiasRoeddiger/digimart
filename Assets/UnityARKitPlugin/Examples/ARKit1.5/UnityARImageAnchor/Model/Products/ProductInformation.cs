using System;
using System.Collections.Generic;
using System.ComponentModel;

public partial class Product
{

    public double Calories { get; set; }
    public double Fat { get; set; }
    public double Sugar { get; set; }
    public double Salt { get; set; }

    [DisplayName("Contains Milk")]
    public bool ContainsMilk { get; set; }
    [DisplayName("Contains Nuts")]
    public bool ContainsNuts { get; set; }

    public static Dictionary<string, Range> Ranges = new Dictionary<string, Range>() {
        { "Calories", new Range() { Min = 300, Max = 420 } },
        { "Fat", new Range() { Min = 0, Max = 30 } },
        { "Salt", new Range() { Min = 0.01, Max = 1 } },
        { "Sugar", new Range() { Min = 0, Max = 55 } },
        { "Price", new Range() { Min = 0.3, Max = 3 } },
    };

    public static Func<Product, double> GetPropertySelector(string propertyName)
    {
        switch (propertyName)
        {
            case "Calories":
                return p => p.Calories;
            case "Fat":
                return p => p.Fat;
            case "Salt":
                return p => p.Salt;
            case "Sugar":
                return p => p.Sugar;
            case "Price":
                return p => p.Price;
            default:
                return null;
        }
    }
}

public struct Range
{
    public double Min { get; set; }
    public double Max { get; set; }
}

