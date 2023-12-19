#if TOOLS
using Godot;

namespace Lavos.Plugins.Google.AdMob;

[Tool]
public sealed partial class AdMobEditorExportPlugin : EditorExportPlugin
{
    public override string _GetName() => GooglePlugins.PluginNames.AdMob;

    public override bool _SupportsPlatform(EditorExportPlatform platform)
    {
        return platform is EditorExportPlatformAndroid || platform is EditorExportPlatformIos;
    }

    public override string[] _GetAndroidLibraries(EditorExportPlatform platform, bool debug)
    {
        var aar = $"{_GetName()}.{(debug ? "debug" : "release")}.aar";
        return new string[]
        {
            $"{GooglePlugins.Root}/admob/.bin/android/{aar}"
        };
    }

    public override string[] _GetAndroidDependencies(EditorExportPlatform platform, bool debug)
    {
        return new[] 
        {
            "com.google.android.gms:play-services-ads:22.2.0",
            "org.jetbrains.kotlinx:kotlinx-serialization-json-jvm:1.4.1"
        };
    }
}
#endif