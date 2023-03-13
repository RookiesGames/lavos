using System;

namespace Lavos.Utils.Automation
{
    public interface IStackState: IState
    {
        Action Resume { get; set; }
        Action Pause { get; set; }
    }
}