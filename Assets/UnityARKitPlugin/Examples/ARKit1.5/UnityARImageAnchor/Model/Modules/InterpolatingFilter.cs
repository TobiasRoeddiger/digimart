using System;
using UnityEngine;

public class InterpolatingFilter : IDigiFilter
{
    private Func<Product, double> _propertySelector { get; set; }
    private double _min { get; set; }
    private double _max { get; set; }

    private static readonly Color MIN_COLOR = new Color(0.2f, 0.82f, 0.33f, 0.6f);
    private static readonly Color MAX_COLOR = new Color(0.82f, 0.28f, 0.2f, 0.6f);

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
            InterpolateComponent(MIN_COLOR.r, MAX_COLOR.r, a),
            InterpolateComponent(MIN_COLOR.g, MAX_COLOR.g, a),
            InterpolateComponent(MIN_COLOR.b, MAX_COLOR.b, a),
            InterpolateComponent(MIN_COLOR.a, MAX_COLOR.a, a)
        );
        return color;
    }

    private static float InterpolateComponent(double a, double b, double lambda)
    {
        return (float) (a + (b - a) * lambda);
    }
}
