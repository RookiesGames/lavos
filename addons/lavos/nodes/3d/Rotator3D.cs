using Godot;

namespace Lavos.Nodes;

public partial class Rotator3D : Node3D
{
    public enum Axis { X, Y, Z };
    [Export] Axis RotationAxis;
    [Export(PropertyHint.Range, "0,3,0.1")] double RotationSpeed = 5;

    public int RotationDirection { get; set; } = 1;

    public override void _Process(double delta)
    {
        float angle = (float)(delta * RotationSpeed * RotationDirection);
        switch (RotationAxis)
        {
            case Axis.X: RotateX(angle); break;
            case Axis.Y: RotateY(angle); break;
            case Axis.Z: RotateZ(angle); break;
            default: break;
        }
    }
}