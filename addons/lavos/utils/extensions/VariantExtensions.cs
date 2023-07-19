using Godot;

namespace Lavos.Utils.Extensions;

public static class VariantExtensions
{
    public static float AsFloat(this Variant variant) => (float)variant.AsDouble();
}
