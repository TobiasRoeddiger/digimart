using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AllergyFilter : IDigiFilter
{
    private IList<Func<Product, bool>> _propertySelectors { get; set; }

    private static readonly Color _GOOD_COLOR = new Color(50, 209, 84, 255);
    private static readonly Color _NOTGOOD_COLOR = new Color(209, 71, 50, 25);
    
    public AllergyFilter(IList<Func<Product, bool>> propertySelectors)
    {
        _propertySelectors = propertySelectors;
    }


    public Color CalculateOverlayColor(Product product)
    {
        return _propertySelectors.Any(selector => selector(product)) ? _NOTGOOD_COLOR : _GOOD_COLOR;
    }
}
