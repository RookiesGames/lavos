
using Godot;
using System;
using Lavos.Utils.Automation;

namespace Lavos.UI;

internal sealed class FadeInState : BaseFadeState
{
    public event Action FadeInCompleted;

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
        Log.Debug("1");
        FadeInCompleted?.Invoke();
        Log.Debug("2");
        FadeInCompleted = null;
        //
        _panel.Visible = false;
    }

    #endregion
}
