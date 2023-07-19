using Godot;

namespace Lavos.Nodes.Camera;

[GlobalClass]
public partial class CameraConfig : Resource
{
    [Export] float _height;
    public float Height => _height;

    [Export] float _distance;
    public float Distance => _distance;
}
