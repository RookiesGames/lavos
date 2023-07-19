using Godot;
using System.Collections.Generic;

namespace Lavos.Utils.Extensions;

public static class DirectoryExtensions
{
    const string Tag = nameof(Godot.DirAccess);

    public static List<string> GetFilesInDirectory(this Godot.DirAccess dir)
    {
        var error = dir.ListDirBegin();
        if (error != Error.Ok)
        {
            return null;
        }
        //
        var list = new List<string>();
        var wd = dir.GetCurrentDir();
        while (true)
        {
            var filename = dir.GetNext();
            if (string.IsNullOrEmpty(filename))
            {
                break;
            }
            //
            if (filename == "." || filename == "..")
            {
                continue;
            }
#if GODOT_MACOS
            if (filename == ".DS_Store")
            {
                continue;
            }
#endif
            list.Add(System.IO.Path.Combine(wd, filename));
        }
        //
        list.Sort();
        return list;
    }

    public static void RemoveDirectory(this Godot.DirAccess dir, string path)
    {
        var ok = dir.ListDirBegin();
        if (ok != Error.Ok)
        {
            Log.Error(Tag, $"Failed to list directory {path}");
            return;
        }

        var wd = dir.GetCurrentDir();
        while (true)
        {
            var entry = dir.GetNext();
            if (string.IsNullOrEmpty(entry))
            {
                break;
            }
            //
            if (entry == "." || entry == "..")
            {
                continue;
            }
            //
            if (dir.CurrentIsDir())
            {
                dir.RemoveDirectory($"{wd}/{entry}");
            }
            else
            {
                dir.Remove($"{wd}/{entry}");
            }
        }

        dir.Remove(path);
    }
}
