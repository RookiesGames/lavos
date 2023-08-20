#if TOOLS
using Godot;

namespace Lavos.Addons.Google.Firebase.Analytics;

[Tool]
public sealed partial class FirebaseAnalyticsEditorPlugin : EditorPlugin
{
    readonly FirebaseAnalyticsEditorExportPlugin Plugin = new();

    public override void _EnterTree() => AddExportPlugin(Plugin);
    public override void _ExitTree() => RemoveExportPlugin(Plugin);
}
#endif