using Godot;

namespace Lavos.Math;

public sealed class Range
{
    float _value;
    public float Value
    {
        get => _value;
        set => _value = Mathf.Clamp(value, Min, Max);
    }
    public float Min { get; }
    public float Max { get; }

    public Range(float value, float min, float max)
    {
        _value = value;
        Min = min;
        Max = max;
    }

    public void Add(float value) => _value = Mathf.Clamp(_value + value, Min, Max);
    public void Substract(float value) => _value = Mathf.Clamp(_value - value, Min, Max);
    public void Multiply(float value) => _value = Mathf.Clamp(_value * value, Min, Max);
    public void Divice(float value) => _value = Mathf.Clamp(_value / value, Min, Max);
}
