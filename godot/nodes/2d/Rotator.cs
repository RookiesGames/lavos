using Godot;

namespace Lavos.Nodes
{
    public sealed class Rotator : Node2D
    {
        [Export] double RotationSpeed = 5;

        float _rotationFactor = 1f;
        public bool Paused
        {
            get => _rotationFactor == 0f;
            set => _rotationFactor = value ? 0f : 1f;
        }

        public override void _Process(float delta)
        {
            Rotate((float)(delta * RotationSpeed * _rotationFactor));
        }
    }
}