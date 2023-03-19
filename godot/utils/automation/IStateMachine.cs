using Lavos.Core;

namespace Lavos.Utils.Automation
{
    public interface IStateMachine : IProcessable
    {
        IState CurrentState { get; }

        void ChangeState(IState state);
    }
}