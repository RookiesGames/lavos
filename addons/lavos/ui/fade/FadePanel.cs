using Godot;
using Lavos.Utils.Automation;
using System.Threading.Tasks;

namespace Lavos.UI;

[GlobalClass]
public sealed partial class FadePanel : ColorRect
{
    [Export] FadePanelResource Config;

    readonly StateMachine _stateMachine = new();
    State _fadeInState;
    State _fadeOutState;

    #region Node

    public override void _Ready()
    {
        this.SetAlpha(Config.Faded ? 1f : 0f);
        Visible = Config.Faded;
        MouseFilter = Config.Faded ? MouseFilterEnum.Stop : MouseFilterEnum.Ignore;
        _fadeInState = new FadeInState(this, Config.FadeInDuration);
        _fadeOutState = new FadeOutState(this, Config.FadeOutDuration);
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

    #endregion

    public async Task FadeInAsync()
    {
        _stateMachine.ChangeState(_fadeInState);
        await Task.Delay((int)(Config.FadeInDuration * 1000));
    }

    public Task FadeIn()
    {
        _stateMachine.ChangeState(_fadeInState);
        return Task.CompletedTask;
    }

    public async Task FadeOutAsync()
    {
        _stateMachine.ChangeState(_fadeOutState);
        await Task.Delay((int)(Config.FadeOutDuration * 1000));
    }

    public Task FadeOut()
    {
        _stateMachine.ChangeState(_fadeOutState);
        return Task.CompletedTask;
    }
}
