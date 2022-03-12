using Godot;

namespace Lavos.Core.Nodes
{
    public sealed class Rotator : Node2D
    {
        [Export] double RotationSpeed = 5;

        double _rotation = 0;
        const double TwoPI = System.Math.PI * 2;

        public override void _Process(float delta)
        {
            // Rotation
            _rotation += (delta * RotationSpeed);
            _rotation %= TwoPI;
            Rotate((float)(delta * RotationSpeed));
        }
    }
}