#if TOOLS
using Godot;

namespace Lavos.Plugins.Google.AdMob;

[Tool]
public sealed partial class AdMobEditorPlugin : EditorPlugin
{
    AdMobEditorExportPlugin Plugin;

    public override void _EnterTree()
    {
        Plugin = new AdMobEditorExportPlugin();
        AddExportPlugin(Plugin);
    }

    public override void _ExitTree()
    {
        RemoveExportPlugin(Plugin);
        Plugin = null;
    }
}
#endif