using System;

namespace Lavos.Utils.Automation
{
    public interface IStackState : IState
    {
        StackStatePhase Phase { get; set; }
        void Resume();
        void Pause();
    }
}