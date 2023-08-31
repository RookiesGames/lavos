using Lavos.Dependency;
using System;
using System.Collections.Generic;

namespace Lavos.Core;

internal sealed class ProcessorService : IProcessorService
{
    HashSet<IProcessable> _processables = new HashSet<IProcessable>();
    public IReadOnlySet<IProcessable> Processables => _processables;

    void IProcessorService.Register(IProcessable processable) => _processables.Add(processable);
    void IProcessorService.Unregister(IProcessable processable) => _processables.Remove(processable);
}