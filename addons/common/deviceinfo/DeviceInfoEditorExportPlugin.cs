#if TOOLS
using Godot;

namespace Lavos.Plugins.Common.DeviceInfo;

[Tool]
public sealed partial class DeviceInfoEditorExportPlugin : EditorExportPlugin
{
    public override string _GetName() => "DeviceInfo";

    public override bool _SupportsPlatform(EditorExportPlatform platform)
    {
        return platform is EditorExportPlatformAndroid;
    }

    public override string[] _GetAndroidLibraries(EditorExportPlatform platform, bool debug)
    {
        var aar = $"{_GetName()}.{(debug ? "debug" : "release")}.aar";
        return new[]{ $"res://addons/rookies/common/deviceinfo/.bin/android/{aar}" };
    }
}
#endif