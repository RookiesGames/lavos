using Godot;

namespace Lavos.Utils.Extensions;

public static class Node3DExtensions
{
    public static Vector3 GetLocalUp(this Node3D node) => node.Basis.Y;
    public static Vector3 GetLocalForward(this Node3D node) => -1 * node.Basis.Z;

    public static Vector3 GetGlobalUp(this Node3D node) => node.GlobalTransform.Basis.Y;
    public static Vector3 GetGlobalForward(this Node3D node) => -1 * node.GlobalTransform.Basis.Z;

    public static void LookAtTarget(this Node3D node, Vector3 target)
    {
        if (!node.Position.IsEqualApprox(target))
        {
            node.LookAt(target);
        }
    }
}