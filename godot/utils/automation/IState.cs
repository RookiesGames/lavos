using System;

namespace Lavos.Utils.Automation
{
    public interface IState
    {
        Action Enter { get; set; }
        Action<float> Process { get; set; }
        Action Exit { get; set; }
    }
}