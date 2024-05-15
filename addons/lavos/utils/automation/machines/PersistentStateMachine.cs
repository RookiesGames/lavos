
using Lavos.Core;
using System.Collections.Generic;

namespace Lavos.Utils.Automation;

public sealed class PersistentStateMachine : IProcessable
{
    const string Tag = nameof(PersistentStateMachine);

    List<PersistentState> _states = new();

    public PersistentState CurrentState { get; private set; }

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
        SwitchState(state);
    }

    void SwitchState(PersistentState state)
    {
        CurrentState?.Exit();
        CurrentState = state;
        CurrentState?.Enter();
    }

    public void Process(double delta)
    {
        CurrentState?.Update(delta);
    }
}