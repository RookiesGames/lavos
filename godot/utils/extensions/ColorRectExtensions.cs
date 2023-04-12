using Godot;

namespace Lavos.Utils.Extensions;

public static class ColorRectExtensions
{
    public static void SetAlpha(this ColorRect tex, float alpha)
    {
        var color = tex.Color;
        color.A = alpha;
        tex.Color = color;
    }
}
