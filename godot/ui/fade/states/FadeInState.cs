using Godot;
using Lavos.Core;
using Lavos.Utils.Automation;
using System;

namespace Lavos.UI;

internal sealed class FadeInState : BaseFadeState, IState
{
    public FadeInState(FadePanel panel, double duration) : base(panel, duration) { }

    #region IState

    public event Action<IState> StateChanged;

    void IState.Enter()
    {
        _timer = 0;
        _panel.SetAlpha(1);
        _panel.MouseFilter = Control.MouseFilterEnum.Stop;
    }

    void IState.Update(double delta)
    {
        _timer += delta;
        var weight = (float)(_timer / _duration);
        var alpha = Mathf.Lerp(1, 0, weight);
        _panel.SetAlpha(alpha);
        //
        if (alpha <= 0)
        {
            _panel.MouseFilter = Control.MouseFilterEnum.Ignore;
            StateChanged?.Invoke(null);
        }
    }

    void IState.Exit() { }

    #endregion
}
