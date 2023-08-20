#if TOOLS
using Godot;

namespace Lavos.Addons.Google.Firebase.Crashlytics;

[Tool]
public sealed partial class FirebaseCrashlyticsEditorPlugin : EditorPlugin
{
    readonly FirebaseCrashlyticsEditorExportPlugin Plugin = new();

    public override void _EnterTree() => AddExportPlugin(Plugin);
    public override void _ExitTree() => RemoveExportPlugin(Plugin);
}
#endif