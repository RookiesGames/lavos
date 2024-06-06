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

    readonly StateMachine _stateMachine = new();
    FadeInState _fadeInState;
    FadeOutState _fadeOutState;

    #region Node

    public override void _EnterTree()
    {
        MouseFilter = MouseFilterEnum.Ignore;
        //
        _fadeInState = new FadeInState(this, Config.FadeInDuration);
        _fadeOutState = new FadeOutState(this, Config.FadeOutDuration);
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
        _stateMachine.ChangeState(_fadeInState);
        await Task.Delay((int)(Config.FadeInDuration * 1000));
    }

    public void FadeIn(Action onCompleted = null)
    {
        _fadeInState.FadeInCompleted += onCompleted;
        Log.Debug(_stateMachine.CurrentState);
        _stateMachine.ChangeState(_fadeInState);
    }

    public async Task FadeOutAsync()
    {
        _stateMachine.ChangeState(_fadeOutState);
        await Task.Delay((int)(Config.FadeOutDuration * 1000));
    }

    public void FadeOut(Action onCompleted = null)
    {
        _fadeOutState.FadeOutCompleted += onCompleted;
        Log.Debug(_stateMachine.CurrentState);
        _stateMachine.ChangeState(_fadeOutState);
    }
}
