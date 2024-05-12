using System;

namespace Lavos.Utils.Automation;

public abstract class StackState
{
    public StackStateMachine StateMachine { get; set; }
    public StackStatePhase Phase { get; set; }

    public virtual void Enter() { }
    public virtual void Resume() { }
    public virtual void Update(double delta) { }
    public virtual void Pause() { }
    public virtual void Exit() { }
}
