
using Lavos.Core;
using System.Collections.Generic;

namespace Lavos.Utils.Automation;

public sealed class PersistentStateMachine : IProcessable
{
    const string Tag = nameof(PersistentStateMachine);

    List<PersistentState> _states = new();

    public PersistentState CurrentState { get; private set; }
    public PersistentState PendingState { get; private set; }

    public T GetState<T>() where T : PersistentState
    {
        foreach (var state in _states)
        {
            if (state is T foundState)
            {
                return foundState;
            }
        }

        return null;
    }

    public void AddState<T>() where T : PersistentState, new()
    {
        AddState(new T());
    }

    public void AddState(PersistentState state)
    {
        if (_states.Contains(state))
        {
            Log.Warn(Tag, "State already present");
            return;
        }
        //
        state.StateMachine = this;
        _states.Add(state);
    }

    public void RemoveState<T>() where T : PersistentState, new()
    {
        var state = _states.Find(s => s.GetType() == typeof(T));
        RemoveState(state);
    }

    public void RemoveState(PersistentState state)
    {
        if (state == CurrentState)
        {
            CurrentState = null;
        }
        //
        _states.Remove(state);
    }

    public void GoToState<T>() where T : PersistentState
    {
        var state = _states.Find(s => s.GetType() == typeof(T));
        if (state == null)
        {
            Log.Error(Tag, $"State {nameof(T)} could not be found");
            return;
        }
        if (state == CurrentState)
        {
            Log.Warn(Tag, $"Machine already in state {nameof(T)}");
            return;
        }
        //
        PendingState = state;
    }

    void SwitchState()
    {
        CurrentState?.Exit();
        CurrentState = PendingState;
        PendingState = null;
        CurrentState?.Enter();
    }

    public void Process(double delta)
    {
        if (PendingState != null)
        {
            SwitchState();
        }
        CurrentState?.Update(delta);
    }
}