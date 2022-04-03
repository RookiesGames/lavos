using System;

namespace Lavos.Utils.Automation
{
    public interface IState
    {
        string Tag { get; }
        Action Enter { get; set; }
        Action<float> Process { get; set; }
        Action Exit { get; set; }
    }
}