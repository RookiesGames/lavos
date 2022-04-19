#if TOOLS

using Godot;
using System;

namespace Lavos.UI
{
    [Tool]
    public class CustomUI : EditorPlugin
    {
        const string Path = "res://addons/lavos/CustomUI";
        public override void _EnterTree()
        {
            var script = GD.Load<Script>($"{Path}/ui/{nameof(ClickButton)}.cs");
            var icon = GD.Load<Texture>($"{Path}/icons/lavos.png");
            AddCustomType(nameof(ClickButton), nameof(Button), script, icon);
        }

        public override void _ExitTree()
        {
            RemoveCustomType(nameof(ClickButton));
        }
    }
}

#endif