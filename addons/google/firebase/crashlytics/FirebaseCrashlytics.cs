#if TOOLS
using Godot;

namespace Lavos.Addons;

[Tool]
public partial class FirebaseCrashlytics : EditorExportPlugin
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