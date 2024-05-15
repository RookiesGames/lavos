
using Godot;
using Lavos.Utils.Automation;

namespace Lavos.UI;

internal sealed class FadeInState : BaseFadeState
{
    public FadeInState(FadePanel panel, double duration) : base(panel, duration) { }

    #region State

    public override void Enter()
    {
        _timer = 0;
        _panel.SetAlpha(1);
        _panel.MouseFilter = Control.MouseFilterEnum.Stop;
        _panel.Visible = true;
    }

    public override void Update(double delta)
    {
        _timer += delta;
        //
        var alpha = (float)Mathf.Lerp(1.0, 0.0, _timer / _duration);
        _panel.SetAlpha(alpha);
        //
        if (alpha <= 0)
        {
            _panel.MouseFilter = Control.MouseFilterEnum.Ignore;
            StateMachine.ChangeState(null);
        }
    }

    public override void Exit()
    {
        _panel.Visible = false;
    }

    #endregion
}
