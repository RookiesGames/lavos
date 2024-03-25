using System;

namespace Lavos.Utils.Automation;

public abstract class IStackState
{
    public StackStatePhase Phase { get; set; }

    public event Action<IStackState> StatePushed;
    public event Action StatePopped;

    public virtual void Enter() { }
    public virtual void Resume() { }
    public virtual void Update(double delta) { }
    public virtual void Pause() { }
    public virtual void Exit() { }

    public void PushState(IStackState state)
    {
        StatePushed?.Invoke(state);
    }

    public void PopState()
    {
        StatePopped?.Invoke();
    }
}
