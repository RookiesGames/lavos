using Godot;
using Lavos.Core;
using Lavos.Utils.Automation;
using System;

namespace Lavos.UI;

internal sealed class FadeOutState : BaseFadeState, IState
{
    public FadeOutState(FadePanel panel, double duration) : base(panel, duration) { }

    #region IState

    public event EventHandler<IState> StateChanged;

    void IState.Enter()
    {
        _timer = 0;
        _panel.SetAlpha(0);
    }

    void IProcessable.Process(double delta)
    {
        _timer += delta;
        var weight = (float)(_timer / _duration);
        var alpha = Mathf.Lerp(0, 1, weight);
        _panel.SetAlpha(alpha);
        //
        if (alpha >= 1)
        {
            StateChanged?.Invoke(this, null);
        }
    }

    void IState.Exit() { }

    #endregion
}