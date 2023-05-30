using Godot;
using Lavos.Dependency;

namespace Lavos.Core;

sealed partial class Processor : Node
{
    ProcessorService _service;

    public override void _Ready()
    {
        _service = ServiceLocator.Locate<IProcessorService>() as ProcessorService;
    }

    public override void _Process(double delta)
    {
        foreach (var process in _service.Processables)
        {
            process.Process(delta);
        }
    }
}