using System;

namespace Lavos.Utils.Automation
{
    public sealed class State : IState
    {
        public Action Enter { get; set; }
        public Action<float> Process { get; set; }
        public Action Exit { get; set; }
    }
}