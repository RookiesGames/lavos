using Godot;

namespace Lavos.Utils.Extensions
{
    public static class PackedSceneExtensions
    {
        public static T Instance<T>(this PackedScene scene, Node parent) where T : Node
        {
            var node = scene.Instance<T>();
            parent.AddChild(node);
            return node;
        }
    }
}