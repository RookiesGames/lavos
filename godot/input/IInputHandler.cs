using System;
using Lavos.Input;

namespace Lavos.Input
{
    public interface IInputHandler<C> where C : IInputConfig
    {
        void EnableHandler(C config);
        void DisableHandler();
    }
}