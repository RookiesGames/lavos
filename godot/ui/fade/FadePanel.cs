using Godot;
using Lavos.Utils.Automation;
using System.Threading.Tasks;

namespace Lavos.UI;

public sealed partial class FadePanel : ColorRect
{
    [Export] FadePanelResource Config;

    readonly IStateMachine _stateMachine = new StateMachine(null);
    IState _fadeInState;
    IState _fadeOutState;

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
        _stateMachine.Dispose();
    }

    #endregion

    public async Task FadeIn()
    {
        _stateMachine.ChangeState(_fadeInState);
        await Task.Delay((int)(Config.FadeInDuration * 1000));
    }

    public async Task FadeOut()
    {
        _stateMachine.ChangeState(_fadeOutState);
        await Task.Delay((int)(Config.FadeOutDuration * 1000));
    }
}
