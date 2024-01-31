using Godot;

namespace Lavos.Math;

public sealed class Range
{
    double _value;
    public double Value
    {
        get => _value;
        set => _value = Mathf.Clamp(value, Min, Max);
    }
    public double Min { get; }
    public bool IsMin => _value == Min;
    public double Max { get; }
    public bool IsMax => _value == Max;

    public Range(double value, double min, double max)
    {
        _value = value;
        Min = min;
        Max = max;
    }

    public void Add(double value) => _value = Mathf.Clamp(_value + value, Min, Max);
    public void Substract(double value) => _value = Mathf.Clamp(_value - value, Min, Max);
    public void Multiply(double value) => _value = Mathf.Clamp(_value * value, Min, Max);
    public void Divice(double value) => _value = Mathf.Clamp(_value / value, Min, Max);
}
