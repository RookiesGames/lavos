#if TOOLS

using Godot;

namespace Lavos;

[Tool]
public sealed partial class Lavos : EditorPlugin
{
    const string Path = "res://addons/lavos/";
    public override void _EnterTree()
    {
        //var script = GD.Load<Script>($"{Path}/ui/{nameof(ClickButton)}.cs");
        //var icon = GD.Load<Texture2D>($"{Path}/icons/lavos.png");
        //AddCustomType(nameof(ClickButton), nameof(Button), script, icon);
    }

    public override void _ExitTree()
    {
        RemoveCustomType(nameof(ClickButton));
    }
}

#endif