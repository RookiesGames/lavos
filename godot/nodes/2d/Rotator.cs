using Godot;

namespace Lavos.Nodes;

public sealed partial class Rotator : Node2D
{
    [Export] double RotationSpeed = 5;

    float _rotationFactor = 1f;
    public bool Paused
    {
        get => _rotationFactor == 0f;
        set => _rotationFactor = value ? 0f : 1f;
    }

    public override void _Process(double delta)
    {
        Rotate((float)(delta * RotationSpeed * _rotationFactor));
    }
}
