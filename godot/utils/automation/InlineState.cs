using System;

namespace Lavos.Utils.Automation
{
    public sealed class InlineState : IState
    {
        #region IState

        public event EventHandler<IState> StateChanged;

        public Action Enter;
        public Action<double> Process;
        public Action Exit;

        void IState.Enter() => Enter?.Invoke();
        void IState.Process(double dt) => Process?.Invoke(dt);
        void IState.Exit() => Exit?.Invoke();

        #endregion
    }
}