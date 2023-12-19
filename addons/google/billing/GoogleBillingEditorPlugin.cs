#if TOOLS
using Godot;

namespace Lavos.Plugins.Google.Billing;

[Tool]
public sealed partial class GoogleBillingEditorPlugin : EditorPlugin
{
    GoogleBillingExportEditorPlugin Plugin;

    public override void _EnterTree()
    {
        Plugin = new GoogleBillingExportEditorPlugin();
        AddExportPlugin(Plugin);
    }

    public override void _ExitTree()
    {
        RemoveExportPlugin(Plugin);
        Plugin = null;
    }
}
#endif