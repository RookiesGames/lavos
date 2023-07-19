using Godot;

namespace Lavos.Utils.Extensions;

public static class SpriteExtensions
{
    public static void SetAlpha(this Sprite2D sprite, float alpha)
    {
        var color = sprite.Modulate;
        color.A = alpha;
        sprite.Modulate = color;
    }
}