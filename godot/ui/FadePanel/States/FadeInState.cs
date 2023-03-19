using Godot;
using Lavos.Utils.Automation;
using Lavos.Utils.Extensions;
using System;

namespace Lavos.UI
{
    internal sealed class FadeInState : BaseFadeState, IState
    {
        public FadeInState(FadePanel panel, double duration) : base(panel, duration) { }

        #region IState

        public event EventHandler<IState> StateChanged;

        void IState.Enter()
        {
            _timer = 0;
            _panel.SetAlpha(1);
        }

        void IState.Process(double dt)
        {
            _timer += dt;
            var weight = (float)(_timer / _duration);
            var alpha = Mathf.Lerp(1, 0, weight);
            _panel.SetAlpha(alpha);
            //
            if (alpha <= 0)
            {
                StateChanged?.Invoke(this, null);
            }
        }

        void IState.Exit() { }

        #endregion
    }
}