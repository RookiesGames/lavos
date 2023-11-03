#if TOOLS
using Godot;

namespace Lavos.Plugins.Google.PlayGames;

[Tool]
public sealed partial class GooglePlayGamesEditorPlugin : EditorPlugin
{
    GooglePlayGamesEditorExportPlugin Plugin;

    public override void _EnterTree()
    {
        Plugin = new GooglePlayGamesEditorExportPlugin();
        AddExportPlugin(Plugin);
    }

    public override void _ExitTree()
    {
        RemoveExportPlugin(Plugin);
        Plugin = null;
    }
}
#endif