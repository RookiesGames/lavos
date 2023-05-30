using Lavos.Core;
using System;

namespace Lavos.Utils.Automation;

public interface IStateMachine : IProcessable, IDisposable
{
    event Action<IState> StateChanged;

    IState CurrentState { get; }

    void ChangeState(IState state);
}
