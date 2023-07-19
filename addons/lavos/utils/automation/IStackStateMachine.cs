using System;
using Lavos.Core;

namespace Lavos.Utils.Automation;

public interface IStackStateMachine : IProcessable, IDisposable
{
    IStackState CurrentState { get; }

    void PushState(IStackState state);
    void PopState();
}