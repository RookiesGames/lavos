using Lavos.Core;

namespace Lavos.Utils.Automation
{
    public sealed class StateMachine : IStateMachine
    {
        IState _pendingState = null;

        bool _pendingTransition = false;
        bool HasPendingState => _pendingTransition;

        #region IStateMachine

        IState _state = null;
        public IState CurrentState => _state;

        void IStateMachine.ChangeState(IState state)
        {
            OnStateChanged(this, state);
        }

        void OnStateChanged(object sender, IState state)
        {
            _pendingState = state;
            _pendingTransition = true;
        }

        void IProcessable.Process(double dt)
        {
            if (HasPendingState)
            {
                SwitchState();
            }
            _state?.Process(dt);
        }

        void SwitchState()
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