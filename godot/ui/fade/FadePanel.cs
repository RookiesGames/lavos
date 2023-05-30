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

    readonly IStateMachine _stateMachine = new StateMachine();
    IState _fadeInState;
    IState _fadeOutState;

    bool IsFadingIn => _stateMachine.CurrentState == _fadeInState;
    bool IsFadingOut => _stateMachine.CurrentState == _fadeOutState;

    public event Action FadedIn;
    public event Action FadedOut;

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
    }

    public override void _Process(double delta)
    {
        _stateMachine.Process(delta);
    }

    public Task FadeIn()
    {
        _stateMachine.ChangeState(_fadeInState);
        await Task.w
        return Task.Run(async () =>
        {
            TimeSpan timeSpan = TimeSpan.FromMilliseconds(60);
            while (IsFadingIn)
            {
                await Task.Delay(timeSpan);
            }
            _stateMachine.ChangeState(null);
            return Task.CompletedTask;
        });
    }

    public Task FadeOut()
    {
        _stateMachine.ChangeState(_fadeOutState);
        return Task.Run(async () =>
        {
            TimeSpan timeSpan = TimeSpan.FromMilliseconds(60);
            while (IsFadingOut)
            {
                await Task.Delay(timeSpan);
            }
            _stateMachine.ChangeState(null);
            return Task.CompletedTask;
        });
    }
}
