using System;

namespace Lavos.Utils.Automation
{
    public sealed class State : IState
    {
        public string Tag { get; set; }
        public Action Enter { get; set; }
        public Action<double> Process { get; set; }
        public Action Exit { get; set; }
    }
}