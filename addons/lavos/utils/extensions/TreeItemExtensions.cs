using Godot;

namespace Lavos.Utils.Extensions;

public static class TreeItemExtensions
{
    public static int GetHierarchyHeight(this TreeItem item)
    {
        var height = 0;
        var parent = item.GetParent();
        while (parent != null)
        {
            ++height;
            parent = parent.GetParent();
        }
        return height;
    }
}