using Lavos.Core;
using System;

namespace Lavos.Utils.Automation;

public sealed class StateMachine : IProcessable
{
    public event Action<State> StateChanged;

    public State CurrentState { get; private set; }

    public void ChangeState(State state)
    {
        CurrentState?.Exit();
        CurrentState = state;
        if (CurrentState != null)
        {
            CurrentState.StateMachine = this;
        }
        CurrentState?.Enter();
        StateChanged?.Invoke(CurrentState);
    }

    public void Process(double delta)
    {
        CurrentState?.Update(delta);
    }
}
