using Lavos.Dependency;
using System;

namespace Lavos.Core;

public interface IProcessorService : IService
{
    void Register(IProcessable processable);
    void Unregister(IProcessable processable);
}