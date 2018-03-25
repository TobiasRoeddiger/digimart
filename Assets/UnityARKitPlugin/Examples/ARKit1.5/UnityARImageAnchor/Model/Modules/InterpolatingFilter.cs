using System;
using UnityEngine;

public class InterpolatingFilter : IDigiFilter
{
    private Func<Product, double> _propertySelector { get; set; }
    private double _min { get; set; }
    private double _max { get; set; }

    private static readonly Color MIN_COLOR = new Color(0.36f, 0.98f, 0.24f, 0.8f);
    private static readonly Color MIDDLE_COLOR = new Color(1f, 0.93f, 0.19f, 0.8f);
    private static readonly Color MAX_COLOR = new Color(0.98f, 0.3f, 0.25f, 0.8f);

    public InterpolatingFilter(Func<Product, double> propertySelector, double min, double max)
    {
        _propertySelector = propertySelector;
        _min = min;
        _max = max;
    }


    public Color CalculateOverlayColor(Product product)
    {
        var val = _propertySelector(product);
        var a = (val - _min) / (_max - _min);

        a = a < 0 ? 0 : a;
        a = a > 1 ? 1 : a;
        
        var color = new Color(
            InterpolateComponent(c => c.r, a),
            InterpolateComponent(c => c.g, a),
            InterpolateComponent(c => c.b, a),
            InterpolateComponent(c => c.a, a)
        );
        return color;
    }

    private static float InterpolateComponent(Func<Color, double> dim, double lambda)
    {
        var a = dim(MIN_COLOR);
        var b = dim(MAX_COLOR);

        if (lambda < 0.5)
        {
            b = dim(MIDDLE_COLOR);
            lambda = lambda * 2;
        }
        else
        {
            a = dim(MIDDLE_COLOR);
            lambda = lambda * 2 - 1;
        }

        return (float) (a + (b - a) * lambda);
    }
}
