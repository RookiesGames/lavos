using Godot;

namespace Lavos.Nodes.Camera;

public abstract partial class BaseCamera : Camera3D
{
    [Export] CameraConfig _config;
    protected CameraConfig Config => _config;
}
