
namespace Lavos.Utils.Automation;

public abstract class State
{
    public StateMachine StateMachine { get; set; }

    public virtual void Enter() { }
    public virtual void Update(double delta) { }
    public virtual void Exit() { }
}
