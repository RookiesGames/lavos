using Godot;

namespace Lavos.Nodes.Nodes3D;

[GlobalClass]
public partial class CameraConfig : Resource
{
    [Export] float _height;
    public float Height => _height;

    [Export] float _distance;
    public float Distance => _distance;
}
