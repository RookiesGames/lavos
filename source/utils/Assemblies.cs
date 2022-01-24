using Godot;
using System;
using System.Linq;
using System.Reflection;

namespace Vortico.Utils
{
    public static class Assemblies
    {
        public static Assembly GetMainAssembly()
        {
            var appName = (string)ProjectSettings.GetSetting("application/config/name");
            var assembly = AppDomain.CurrentDomain.GetAssemblies().First(a => a.FullName.StartsWith(appName));
            return assembly;
        }
    }
}