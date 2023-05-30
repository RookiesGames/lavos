using Godot;
using Lavos.Utils.Automation;
using System;
using System.Threading.Tasks;

namespace Lavos.UI;

public sealed partial class FadePanel : ColorRect
{
    [Export] bool Faded;
    [Export] double FadeInDuration = 1;
    [Export] double FadeOutDuration = 1;

    readonly IStateMachine _stateMachine = new StateMachine(null);
    IState _fadeInState;
    IState _fadeOutState;

    #region Node

    public override void _Ready()
    {
        this.SetAlpha(Faded ? 1f : 0f);
        MouseFilter = Faded ? MouseFilterEnum.Stop : MouseFilterEnum.Ignore;
        _fadeInState = new FadeInState(this, FadeInDuration);
        _fadeOutState = new FadeOutState(this, FadeOutDuration);
        //
        NodeTree.PinNodeByType<FadePanel>(this);
    }

    public override void _ExitTree()
    {
        NodeTree.UnpinNodeByType<FadePanel>();
        _stateMachine.Dispose();
    }

    #endregion

    public async Task FadeIn()
    {
        _stateMachine.ChangeState(_fadeInState);
        await Task.Delay((int)(FadeInDuration * 1000));
    }

    public async Task FadeOut()
    {
        _stateMachine.ChangeState(_fadeOutState);
        await Task.Delay((int)(FadeOutDuration * 1000));
    }
}
