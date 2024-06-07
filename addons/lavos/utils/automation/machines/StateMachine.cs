using Lavos.Core;

namespace Lavos.Utils.Automation;

public sealed class StateMachine : IProcessable
{
    public State CurrentState { get; private set; }
    public State PendingState { get; private set; }

    public void ChangeState<T>() where T : State, new()
    {
        ChangeState(new T());
    }

    public void ChangeState(State state)
    {
        PendingState = state;
    }

    public void Process(double delta)
    {
        if (PendingState != null)
        {
            SwitchState();
        }
        CurrentState?.Update(delta);
    }

    void SwitchState()
    {
        CurrentState?.Exit();
        //
        CurrentState = PendingState;
        PendingState = null;
        //
        if (CurrentState != null)
        {
            CurrentState.StateMachine = this;
        }
        CurrentState?.Enter();
    }
}
