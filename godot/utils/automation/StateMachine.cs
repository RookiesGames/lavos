using Lavos.Core.Console;

namespace Lavos.Utils.Automation
{
    public sealed class StateMachine
    {
        private IState _state = null;
        private IState _pendingState = null;
        private bool _transitionPending;

        private bool HasPendingState => _transitionPending;

        public void ChangeState(State state)
        {
            _pendingState = state;
            _transitionPending = true;
        }

        public void Process()
        {
            _state?.Process?.Invoke();
            if (HasPendingState)
            {
                SwitchState();
            }
        }

        private void SwitchState()
        {
            _state?.Clean?.Invoke();
            //
            _state = _pendingState;
            _pendingState = null;
            //
            _state?.Ready?.Invoke();
            //
            _transitionPending = false;
        }
    }
}