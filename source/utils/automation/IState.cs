using System;

namespace Vortico.Utils.Automation
{
    public interface IState
    {
        Action Ready { get; set; }
        Action Process { get; set; }
        Action Clean { get; set; }
    }
}