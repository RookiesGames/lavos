#if TOOLS
using Godot;
using Lavos.Plugins.Google.Firebase.Crashlytics;

namespace Lavos.Addons.Google.Firebase.Crashlytics;

[Tool]
public sealed partial class FirebaseCrashlyticsEditorExportPlugin : EditorExportPlugin
{
    public override string _GetName() => nameof(FirebaseCrashlytics);

    public override bool _SupportsPlatform(EditorExportPlatform platform)
    {
        return platform is EditorExportPlatformAndroid || platform is EditorExportPlatformIos;
    }

    public override string[] _GetAndroidLibraries(EditorExportPlatform platform, bool debug)
    {
        var aar = $"FirebaseCrashlytics.{(debug ? "debug" : "release")}.aar";
        return new string[]
        {
            $"res://addons/rookies/google/firebase/crashlytics/.bin/android/{aar}"
        };
    }

    public override string[] _GetAndroidDependencies(EditorExportPlatform platform, bool debug)
    {
        return new string[]
        {
            "com.google.firebase:firebase-analytics-ktx:21.3.0",
            "com.google.firebase:firebase-crashlytics-ktx:18.4.0",
            "com.google.firebase:firebase-crashlytics-ndk:18.4.0",
        };
    }
}
#endif
