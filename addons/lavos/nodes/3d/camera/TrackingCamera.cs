using Godot;

namespace Lavos.Nodes.Camera;

[GlobalClass]
public partial class TrackingCamera : BaseCamera
{
    [Export] public Node3D Target;

    public override void _Process(double delta)
    {
        base._Process(delta);

        // Track target
    }
}