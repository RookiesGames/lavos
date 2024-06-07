using Godot;
using Lavos.Utils.Automation;
using System;
using System.Threading.Tasks;

namespace Lavos.UI;

[GlobalClass]
public sealed partial class FadePanel : ColorRect
{
    [Export]
    FadePanelConfig Config;

    readonly PersistentStateMachine _stateMachine = new();

    #region Node

    public override void _EnterTree()
    {
        MouseFilter = MouseFilterEnum.Ignore;
        //
        _stateMachine.AddState(new FadeIdleState());
        _stateMachine.AddState(new FadeInState(this, Config.FadeInDuration));
        _stateMachine.AddState(new FadeOutState(this, Config.FadeOutDuration));
        _stateMachine.GoToState<FadeIdleState>();
    }
    public override void _Ready()
    {
        NodeTree.PinNodeByType<FadePanel>(this);
    }

    public override void _ExitTree()
    {
        NodeTree.UnpinNodeByType<FadePanel>();
    }

    public override void _Process(double delta)
    {
        _stateMachine.Process(delta);
    }

    #endregion

    public async Task FadeInAsync()
    {
        _stateMachine.GoToState<FadeInState>();
        await Task.Delay((int)(Config.FadeInDuration * 1000));
    }

    public void FadeIn(Action onCompleted = null)
    {
        _stateMachine.GetState<FadeInState>().FadeInCompleted += onCompleted;
        _stateMachine.GoToState<FadeInState>();
    }

    public async Task FadeOutAsync()
    {
        _stateMachine.GoToState<FadeOutState>();
        await Task.Delay((int)(Config.FadeOutDuration * 1000));
    }

    public void FadeOut(Action onCompleted = null)
    {
        _stateMachine.GetState<FadeOutState>().FadeOutCompleted += onCompleted;
        _stateMachine.GoToState<FadeOutState>();
    }
}
