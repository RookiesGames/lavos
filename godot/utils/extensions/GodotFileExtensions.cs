using Lavos.Console;
using System.Text;

namespace Lavos.Utils.Extensions
{
    public static class GodotFileExtensions
    {
        static readonly string Tag = nameof(Godot.File);

        public static string GetFilename(this Godot.File file)
        {
            var path = file.GetPath();
            //
            var end = path.LastIndexOf('/') + 1;
            var name = path.Substring(end);
            //
            end = name.LastIndexOf('.');
            name = name.Substring(0, end);
            //
            return name;
        }

        public static string GetFileExtension(this Godot.File file)
        {
            var path = file.GetPath();
            var end = path.LastIndexOf('.') + 1;
            var ext = path.Substring(end);
            return ext;
        }
    }
}