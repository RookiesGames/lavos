using System;

namespace Lavos.Utils.Threading
{
    public interface IThreadDispatcher
    {
        void AddAction(Action action);
    }
}