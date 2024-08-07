#if TOOLS
using Godot;

namespace Lavos.Plugins.Google.Firebase.Analytics;

[Tool]
public sealed partial class FirebaseAnalyticsEditorExportPlugin : EditorExportPlugin
{
    public override string _GetName() => GooglePlugins.PluginNames.FirebaseAnalytics;

    public override bool _SupportsPlatform(EditorExportPlatform platform)
    {
        return platform is EditorExportPlatformAndroid || platform is EditorExportPlatformIos;
    }

    public override string[] _GetAndroidLibraries(EditorExportPlatform platform, bool debug)
    {
        var aar = $"{_GetName()}.{(debug ? "debug" : "release")}.aar";
        return new[]
        {
            $"{GooglePlugins.Root}/firebase/analytics/.bin/android/{aar}"
        };
    }

    public override string[] _GetAndroidDependencies(EditorExportPlatform platform, bool debug)
    {
        return new[]
        {
            "com.google.firebase:firebase-analytics-ktx:21.3.0",
            "androidx.appcompat:appcompat:1.6.1",
            "org.jetbrains.kotlin:kotlin-stdlib:1.7.0"
        };
    }
}
#endif
