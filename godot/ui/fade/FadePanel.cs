using Godot;
using Lavos.Utils.Automation;
using System.Threading.Tasks;

namespace Lavos.UI;

public sealed partial class FadePanel : ColorRect
{
    [Export] bool Faded = false;
    [Export] double FadeInDuration = 1;
    [Export] double FadeOutDuration = 1;

    readonly IStateMachine _stateMachine = new StateMachine();
    IState _fadeInState = null;
    IState _fadeOutState = null;

    bool IsFadingIn => _stateMachine.CurrentState == _fadeInState;
    bool IsFadingOut => _stateMachine.CurrentState == _fadeOutState;

    public override void _Ready()
    {
        this.SetAlpha(Faded ? 1f : 0f);
        _fadeInState = new FadeInState(this, FadeInDuration);
        _fadeOutState = new FadeOutState(this, FadeOutDuration);
    }

    public override void _Process(double delta)
    {
        _stateMachine.Process(delta);
    }

    public Task FadeIn()
    {
        _stateMachine.ChangeState(_fadeInState);
        return Task.Run(async () =>
        {
            System.TimeSpan timeSpan = System.TimeSpan.FromMilliseconds(60);
            while (IsFadingIn)
            {
                await Task.Delay(timeSpan);
            }
            return Task.CompletedTask;
        });
    }

    public Task FadeOut()
    {
        _stateMachine.ChangeState(_fadeOutState);
        return Task.Run(async () =>
        {
            System.TimeSpan timeSpan = System.TimeSpan.FromMilliseconds(60);
            while (IsFadingOut)
            {
                await Task.Delay(timeSpan);
            }
            return Task.CompletedTask;
        });
    }
}
