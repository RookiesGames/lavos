using Godot;

namespace Lavos.Utils.Extensions
{
    public static class AnimatedSpriteExtensions
    {
        public static bool IsAnimFinished(this AnimatedSprite2D sprite, string anim = "default")
        {
            return sprite.Frame == sprite.Frames.GetFrameCount(anim) - 1;
        }
    }
}