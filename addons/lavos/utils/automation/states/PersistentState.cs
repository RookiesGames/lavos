using System;

namespace Lavos.Utils.Automation;

public abstract class PersistentState : State
{
    public new PersistentStateMachine StateMachine { get; set; }
}
