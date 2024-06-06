
using Godot;
using Lavos.Utils.Automation;
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
        Log.Debug(alpha);
        if (alpha >= 1)
        {
            _panel.MouseFilter = Control.MouseFilterEnum.Stop;
            StateMachine.ChangeState(null);
        }
    }

    public override void Exit()
    {
        Log.Debug("3");
        FadeOutCompleted?.Invoke();
        Log.Debug("4");
        FadeOutCompleted = null;
        //
        _panel.Visible = true;
    }

    #endregion
}
