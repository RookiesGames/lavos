#if TOOLS
using Godot;

namespace Lavos.Plugins.Google.Firebase.Crashlytics;

[Tool]
public sealed partial class FirebaseCrashlyticsEditorExportPlugin : EditorExportPlugin
{
    public override string _GetName() => GooglePlugins.PluginNames.FirebaseCrashlytics;

    public override bool _SupportsPlatform(EditorExportPlatform platform)
    {
        return platform is EditorExportPlatformAndroid || platform is EditorExportPlatformIos;
    }

    public override string[] _GetAndroidLibraries(EditorExportPlatform platform, bool debug)
    {
        var aar = $"{_GetName()}.{(debug ? "debug" : "release")}.aar";
        return new[]
        {
            $"{GooglePlugins.Root}/firebase/crashlytics/.bin/android/{aar}"
        };
    }

    public override string[] _GetAndroidDependencies(EditorExportPlatform platform, bool debug)
    {
        return new[]
        {
            "com.google.firebase:firebase-analytics-ktx:21.3.0",
            "com.google.firebase:firebase-crashlytics-ktx:18.4.0",
            "com.google.firebase:firebase-crashlytics-ndk:18.4.0",
        };
    }
}
#endif
