using System;

namespace Lavos.Utils.Automation
{
    public sealed class StackState : IStackState
    {
        public string Tag { get; set; }
        public Action Enter { get; set; }
        public Action<double> Process { get; set; }
        public Action Exit { get; set; }

        public Action Resume { get; set; }
        public Action Pause { get; set; }
    }
}