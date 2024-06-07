
using Godot;
using System;

namespace Lavos.UI;

internal sealed class FadeOutState : BaseFadeState
{
    public event Action FadeOutCompleted;

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
        var alpha = (float)Mathf.Lerp(0.0, 1.0, _timer / _duration);
        _panel.SetAlpha(alpha);
        //
        if (alpha >= 1)
        {
            _panel.MouseFilter = Control.MouseFilterEnum.Stop;
            StateMachine.GoToState<FadeIdleState>();
        }
    }

    public override void Exit()
    {
        FadeOutCompleted?.Invoke();
        FadeOutCompleted = null;
        //
        _panel.Visible = true;
    }

    #endregion
}
