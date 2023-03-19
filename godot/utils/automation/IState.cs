using Lavos.Core;
using System;

namespace Lavos.Utils.Automation
{
    public interface IState : IProcessable
    {
        event EventHandler<IState> StateChanged;

        void Enter();
        void Exit();
    }
}