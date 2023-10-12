#if TOOLS
using Godot;

namespace Lavos.Addons.Google.Firebase.Crashlytics;

[Tool]
public sealed partial class FirebaseCrashlyticsEditorPlugin : EditorPlugin
{
    FirebaseCrashlyticsEditorExportPlugin Plugin;

    public override void _EnterTree()
    {
        Plugin = new FirebaseCrashlyticsEditorExportPlugin();
        AddExportPlugin(Plugin);
    }

    public override void _ExitTree()
    {
        RemoveExportPlugin(Plugin);
        Plugin = null;
    }
}
#endif
