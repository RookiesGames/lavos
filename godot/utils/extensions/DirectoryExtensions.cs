using Godot;
using Lavos.Console;

namespace Lavos.Utils.Extensions
{
    public static class DirectoryExtensions
    {
        const string Tag = nameof(Directory);

        public static void RemoveDirectory(this Directory dir, string path)
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
}