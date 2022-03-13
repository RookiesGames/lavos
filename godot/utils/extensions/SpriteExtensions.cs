using Godot;

namespace Lavos.Utils.Extensions
{
    public static class SpriteExtensions
    {
        public static void SetAlpha(this Sprite sprite, float alpha)
        {
            var color = sprite.Modulate;
            color.a = alpha;
            sprite.Modulate = color;
        }
    }
}