using System;

namespace Lavos.Utils.Automation
{
    public interface IState
    {
        Action Ready { get; set; }
        Action Process { get; set; }
        Action Clean { get; set; }
    }
}