using Godot;

namespace Lavos.Math;

public sealed class Vector3Helper
{
    public static Vector3 Lerp(Vector3 from, Vector3 to, float weight)
    {
        return from.Lerp(to, weight);
    }

    public static float Distance(Vector3 from, Vector3 to)
    {
        var x = Mathf.Pow(from.X - to.X, 2);
        var y = Mathf.Pow(from.Y - to.Y, 2);
        var z = Mathf.Pow(from.Z - to.Z, 2);
        return Mathf.Sqrt(x + y + z);
    }
}