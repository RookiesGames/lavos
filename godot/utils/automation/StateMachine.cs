using Lavos.Core;

namespace Lavos.Utils.Automation
{
    public sealed class StateMachine : IStateMachine
    {
        IState _pendingState = null;

        bool _pendingTransition = false;
        bool HasPendingState => _pendingTransition;

        public StateMachine(IState initialState = null)
        {
            ChangeState(initialState);
        }

        #region IStateMachine

        IState _state = null;
        public IState CurrentState => _state;

        public void ChangeState(IState state)
        {
            OnStateChanged(this, state);
        }

        private void OnStateChanged(object sender, IState state)
        {
            _pendingState = state;
            _pendingTransition = true;
        }

        public void Process(double dt)
        {
            if (HasPendingState)
            {
                SwitchState();
            }
            _state?.Process(dt);
        }

        private void SwitchState()
        {
            if (_state != null)
            {
                _state.Exit();
                _state.StateChanged -= OnStateChanged;
                _state = null;
            }
            //
            if (_pendingState != null)
            {
                _state = _pendingState;
                _state.StateChanged += OnStateChanged;
                _state.Enter();
            }
            //
            _pendingState = null;
            _pendingTransition = false;
        }

        #endregion
    }
}