using Lavos.Core;
using System;

namespace Lavos.Utils.Automation;

public interface IStackState : IProcessable
{
    StackStatePhase Phase { get; set; }

    event Action<IStackState> StatePushed;
    event Action StatePopped;

    void Enter();
    void Resume();
    void Pause();
    void Exit();
}
