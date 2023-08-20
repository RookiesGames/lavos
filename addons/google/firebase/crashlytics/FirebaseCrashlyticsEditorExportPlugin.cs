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
    public override string[] _GetAndroidDependencies(EditorExportPlatform platform, bool debug)
    {
        Assert.IsTrue(platform is EditorExportPlatformAndroid, "Wrong platform used");
        return new string[] { "res://addons/rookies/google/firebase/crashlytics/lib/android/FirebaseCrashlytics.aar" };
    }
}
#endif