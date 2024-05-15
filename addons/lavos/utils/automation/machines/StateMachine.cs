using Lavos.Core;

namespace Lavos.Utils.Automation;

public sealed class StateMachine : IProcessable
{
    public State CurrentState { get; private set; }

    public void ChangeState<T>() where T : State, new()
    {
        ChangeState(new T());
    }

    public void ChangeState(State state)
    {
        CurrentState?.Exit();
        CurrentState = state;
        if (CurrentState != null)
        {
            CurrentState.StateMachine = this;
        }
        CurrentState?.Enter();
    }

    public void Process(double delta)
    {
        CurrentState?.Update(delta);
    }
}
