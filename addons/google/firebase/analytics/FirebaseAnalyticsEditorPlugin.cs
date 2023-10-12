#if TOOLS
using Godot;

namespace Lavos.Addons.Google.Firebase.Analytics;

[Tool]
public sealed partial class FirebaseAnalyticsEditorPlugin : EditorPlugin
{
    FirebaseAnalyticsEditorExportPlugin Plugin;

    public override void _EnterTree()
    {
        Plugin = new FirebaseAnalyticsEditorExportPlugin();
        AddExportPlugin(Plugin);
    }

    public override void _ExitTree()
    {
        RemoveExportPlugin(Plugin);
        Plugin = null;
    }
}
#endif
