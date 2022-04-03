using Lavos.Console;

namespace Lavos.Utils.Automation
{
    public sealed class StateMachine
    {
        IState _state = null;
        IState _pendingState = null;

        bool _pendingTransition = false;
        bool HasPendingState => _pendingTransition;


        public void ChangeState(IState state)
        {
            _pendingState = state;
            _pendingTransition = true;
        }

        public void Process(float dt)
        {
            if (HasPendingState)
            {
                SwitchState();
            }
            _state?.Process?.Invoke(dt);
        }

        private void SwitchState()
        {
            _state?.Exit?.Invoke();
            //
            _state = _pendingState;
            _pendingState = null;
            _pendingTransition = false;
            //
            _state?.Enter?.Invoke();
        }
    }
}