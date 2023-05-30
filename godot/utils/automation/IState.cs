using System;

namespace Lavos.Utils.Automation;

public interface IState
{
    event Action<IState> StateChanged;

    void Enter();
    void Update(double delta);
    void Exit();
}
