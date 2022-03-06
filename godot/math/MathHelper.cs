
namespace Lavos.Math
{
    public class MathHelper
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
    }
}