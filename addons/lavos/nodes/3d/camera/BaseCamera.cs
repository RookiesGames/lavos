using Godot;

namespace Lavos.Nodes.Camera;

public abstract partial class BaseCamera : Camera3D
{
    [Export] CameraConfig _config;
    protected CameraConfig Config => _config;

    public override void _Ready()
    {
        base._Ready();

        // Set camera
        Position = new Vector3(0f, Config.Height, Config.Distance);
        LookAt(Vector3.Zero, this.GetLocalUp());
    }
}
