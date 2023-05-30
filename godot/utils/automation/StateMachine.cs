using Lavos.Core;
using System;

namespace Lavos.Utils.Automation;

public sealed class StateMachine : IStateMachine
{
    IState _pendingState;

    bool _pendingTransition = false;
    bool HasPendingState => _pendingTransition;

    #region IStateMachine

    public event Action<IState> StateChanged;

    IState _state;
    public IState CurrentState => _state;

    public StateMachine(IState initialState)
    {
        OnStateChanged(initialState);
    }

    void IStateMachine.ChangeState(IState state)
    {
        OnStateChanged(state);
    }

    void OnStateChanged(IState state)
    {
        _pendingState = state;
        _pendingTransition = true;
    }

    void IProcessable.Process(double delta)
    {
        if (HasPendingState)
        {
            SwitchState();
        }
        _state?.Process(delta);
    }

    void SwitchState()
    {
        if (_state != null)
        {
            _state.Exit();
            _state.StateChanged -= OnStateChanged;
        }
        //
        _state = _pendingState;
        _pendingState = null;
        _pendingTransition = false;
        //
        if (_state != null)
        {
            _state.StateChanged += OnStateChanged;
            _state.Enter();
        }
        //
        StateChanged?.Invoke(_state);
    }

    #endregion
}
