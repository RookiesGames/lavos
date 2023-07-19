
namespace Lavos.Utils.Extensions;

public static class GodotFileExtensions
{
    public static string GetFilename(this Godot.FileAccess file)
    {
        var path = file.GetPath();
        //
        var end = path.LastIndexOf('/') + 1;
        var name = path[end..];
        //
        end = name.LastIndexOf('.');
        return name[..end];
    }

    public static string GetFileExtension(this Godot.FileAccess file)
    {
        var path = file.GetPath();
        var end = path.LastIndexOf('.') + 1;
        return path[end..];
    }
}
