using Godot;

namespace Lavos.Math
{
    public sealed class MathHelper
    {
        public struct RangeDef
        {
            public double Min;
            public double Max;
            public bool IncludeLowerBound;
            public bool IncludeUpperBound;
        }

        public static bool InRange(double value, RangeDef range)
        {
            var inRange = true;
            inRange &= (range.IncludeLowerBound)
                            ? value >= range.Min
                            : value > range.Min;
            inRange &= (range.IncludeUpperBound)
                            ? value <= range.Max
                            : value < range.Max;
            return inRange;
        }

        public static Vector2 Lerp(Vector2 from, Vector2 to, float weight)
        {
            return from.LinearInterpolate(to, weight);
        }

        public static Vector3 Lerp(Vector3 from, Vector3 to, float weight)
        {
            return from.LinearInterpolate(to, weight);
        }

        public static Color Lerp(Color from, Color to, float weight)
        {
            return from.LinearInterpolate(to, weight);
        }
    }
}