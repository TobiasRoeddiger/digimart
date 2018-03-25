using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AdFilter : IDigiFilter
{
    private string[] _tags { get; set; }

    private static readonly Color _AD_COLOR = new Color(0.95f, 0.9f, 0.26f, 0.4f);
    private static readonly Color _NOAD_COLOR = new Color(1f, 1f, 1f, 0f);

    private static readonly IList<string> Ads = new List<string>()
    {
        "Riegel"
    };


    public Color CalculateOverlayColor(Product product)
    {
        return Ads.Contains(product.ImageName) ? _AD_COLOR : _NOAD_COLOR;
    }
}
