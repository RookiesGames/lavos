using Lavos.Core;
using System;

namespace Lavos.Utils.Automation
{
    public interface IStackState : IProcessable
    {
        StackStatePhase Phase { get; set; }

        event EventHandler<IStackState> StatePushed;
        event EventHandler StatePopped;

        void Enter();
        void Resume();
        void Pause();
        void Exit();
    }
}