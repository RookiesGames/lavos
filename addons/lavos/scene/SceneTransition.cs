using Godot;

namespace Lavos.Scene;

[GlobalClass]
public sealed partial class SceneTransition : Node
{
    [Export] PackedScene _scene;

    public override void _EnterTree()
    {
        NodeTree.PinNodeByType<SceneTransition>(this);
    }

    public override void _ExitTree()
    {
        NodeTree.UnpinNodeByType<SceneTransition>();
    }

    public void Transition()
    {
        SceneManager.ChangeScene(_scene);
    }
}