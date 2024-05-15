
namespace Lavos.Utils.Automation;

public abstract class StackState : State
{
    public new StackStateMachine StateMachine { get; set; }
    public StatePhase Phase { get; set; }

    public virtual void Resume() { }
    public virtual void Pause() { }
}
