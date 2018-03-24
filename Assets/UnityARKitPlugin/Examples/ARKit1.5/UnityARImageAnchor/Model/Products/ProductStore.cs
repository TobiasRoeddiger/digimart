using System;
using System.Collections.Generic;

public class ProductStore
{

    private static readonly Random R = new Random(123456789);
    private static string NewId
    {
        get
        {
            return R.Next(100000, 1000000000).ToString("X");
        }
    }

    private readonly ICollection<Product> _products;


    public ProductStore()
    {
        _products = new List<Product>(){
            new Product() {
                Id = NewId, ImageName = "Muesli", CompanyName = "Kölln", ProductName = "Müsli", ProductSubType = "Früchte",
                Calories = 369, Fat = 7.6, Sugar = 22, Salt = 0.06, ContainsMilk = true, ContainsNuts = true, Price = 2.99
            },
            new Product() {
                Id = NewId, ImageName = "Frucht", CompanyName = "Sondey", ProductName = "Jaffa Cake", ProductSubType = "Kirsch",
                Calories = 391, Fat = 12, Sugar = 52.3, Salt = 0.3, ContainsMilk = true, ContainsNuts = false, Price = 0.99
            },
            new Product() {
                Id = NewId, ImageName = "Riegel", CompanyName = "Crownfield", ProductName = "Crowni", ProductSubType = "Special Flakes",
                Calories = 417, Fat = 11.5, Sugar = 26, Salt = 1, ContainsMilk = true, ContainsNuts = true, Price = 1.49
            },
            new Product() {
                Id = NewId, ImageName = "Cornflakes", CompanyName = "Crownfield", ProductName = "Haferflocken", ProductSubType = "Zart",
                Calories = 372, Fat = 7, Sugar = 0.7, Salt = 0.02, ContainsMilk = false, ContainsNuts = false, Price = 0.39
            },
            new Product() {
                Id = NewId, ImageName = "Zwieback", CompanyName = "Brandt", ProductName = "Markenzwieback", ProductSubType = "",
                Calories = 395, Fat = 5.2, Sugar = 14, Salt = 0.66, ContainsMilk = true, ContainsNuts = false, Price = 0.89
            },
            new Product() {
                Id = NewId, ImageName = "Kekse", CompanyName = "Sondey", ProductName = "Rondissimo", ProductSubType = "Zartbitter",
                Calories = 538, Fat = 30.6, Sugar = 33.5, Salt = 0.44, ContainsMilk = true, ContainsNuts = false, Price = 0.85
            }
        };
    }


    public ICollection<Product> GetProducts()
    {
        return _products;
    }
}
