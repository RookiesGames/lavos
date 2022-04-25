using Godot;

namespace Lavos.Math
{
    public sealed class Range
    {
        float _value = 0f;
        public float Value
        {
            get => _value;
            set => _value = Mathf.Clamp(value, _min, _max);
        }

        float _min = 0f;
        public float Min => _min;

        float _max = 0f;
        public float Max => _max;


        public Range(float value, float min, float max)
        {
            _value = value;
            _min = min;
            _max = max;
        }

        public void Add(float value)
        {
            _value = Mathf.Clamp(_value + value, _min, _max);
        }

        public void Substract(float value)
        {
            _value = Mathf.Clamp(_value - value, _min, _max);
        }

        public void Multiply(float value)
        {
            _value = Mathf.Clamp(_value * value, _min, _max);
        }

        public void Divice(float value)
        {
            _value = Mathf.Clamp(_value / value, _min, _max);
        }
    }
}