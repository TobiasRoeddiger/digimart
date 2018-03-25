using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AllergyFilter : IDigiFilter
{
    private IList<Func<Product, bool>> _propertySelectors { get; set; }

    private static readonly Color _GOOD_COLOR = new Color(0.2f, 0.82f, 0.33f, 0.45f);
    private static readonly Color _NOTGOOD_COLOR = new Color(0.98f, 0.3f, 0.25f, 0.8f);
    
    public AllergyFilter(IList<Func<Product, bool>> propertySelectors)
    {
        _propertySelectors = propertySelectors;
    }


    public Color CalculateOverlayColor(Product product)
    {
        return _propertySelectors.Any(selector => selector(product)) ? _NOTGOOD_COLOR : _GOOD_COLOR;
    }
}
