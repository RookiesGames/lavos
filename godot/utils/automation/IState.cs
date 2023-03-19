using System;

namespace Lavos.Utils.Automation
{
    public interface IState
    {
        event EventHandler<IState> StateChanged;

        void Enter();
        void Process(double dt);
        void Exit();
    }
}