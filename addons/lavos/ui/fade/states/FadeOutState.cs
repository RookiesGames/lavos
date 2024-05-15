
using Godot;
using Lavos.Utils.Automation;

namespace Lavos.UI;

internal sealed class FadeOutState : BaseFadeState
{
    public FadeOutState(FadePanel panel, double duration) : base(panel, duration) { }

    #region State

    public override void Enter()
    {
        _timer = 0;
        _panel.SetAlpha(0);
        _panel.MouseFilter = Control.MouseFilterEnum.Stop;
        _panel.Visible = true;
    }

    public override void Update(double delta)
    {
        _timer += delta;
        //
        var alpha = (float)Mathf.Lerp(0, 1, _timer / _duration);
        _panel.SetAlpha(alpha);
        //
        if (alpha >= 1)
        {
            _panel.MouseFilter = Control.MouseFilterEnum.Stop;
            StateMachine.ChangeState(null);
        }
    }

    public override void Exit()
    {
        _panel.Visible = true;
    }

    #endregion
}
