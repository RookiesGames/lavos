#if TOOLS
using Godot;

namespace Lavos.Plugins.Google.PlayGames;

[Tool]
public sealed partial class GooglePlayGamesEditorExportPlugin : EditorExportPlugin
{
    public override string _GetName() => GooglePlugins.PlayGamesName;

    public override bool _SupportsPlatform(EditorExportPlatform platform)
    {
        return platform is EditorExportPlatformAndroid;
    }

    public override string[] _GetAndroidLibraries(EditorExportPlatform platform, bool debug)
    {
        var aar = $"{_GetName()}.{(debug ? "debug" : "release")}.aar";
        return new[]{ $"{GooglePlugins.Root}/playgames/.bin/android/{aar}" };
    }

    public override string[] _GetAndroidDependencies(EditorExportPlatform platform, bool debug)
    {
        return new[]
        {
            "com.google.android.gms:play-services-games-v2:17.0.0",
            "com.google.android.gms:play-services-auth:20.6.0",
            "org.jetbrains.kotlinx:kotlinx-serialization-json-jvm:1.4.1",
        };
    }
}
#endif