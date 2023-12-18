#if TOOLS
using Godot;

namespace Lavos.Plugins.Common.DeviceInfo;

[Tool]
public sealed partial class DeviceInfoEditorPlugin : EditorPlugin
{
    DeviceInfoEditorExportPlugin Plugin;

    public override void _EnterTree()
    {
        Plugin = new DeviceInfoEditorExportPlugin();
        AddExportPlugin(Plugin);
    }

    public override void _ExitTree()
    {
        RemoveExportPlugin(Plugin);
        Plugin = null;
    }
}
#endif