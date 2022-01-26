using System;

namespace Lavos.Utils.Automation
{
    public sealed class State : IState
    {
        public Action Ready { get; set; }
        public Action Process { get; set; }
        public Action Clean { get; set; }
    }
}