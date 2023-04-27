using Godot;

namespace Lavos.Utils.Extensions;

public static class Node3DExtensions
{
    public static Vector3 GetForwardVector(this Node3D node)
    {
        return -1 * node.Basis.Z;
    }

    public static void LookAtTarget(this Node3D node, Vector3 target)
    {
        if (!node.Position.IsEqualApprox(target))
        {
            node.LookAt(target);
        }
    }
}