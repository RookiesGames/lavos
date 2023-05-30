using System.ComponentModel;
using Godot;
using Lavos.Dependency;
using Lavos.Nodes;

namespace Lavos.Core;

sealed partial class ProcessorConfig : Config
{
    public override void Configure(IDependencyBinder binder)
    {
        binder.Bind<IProcessorService, ProcessorService>();
    }

    public override void Initialize(IDependencyResolver resolver)
    {
        OmniNode.Instance.AddNode<Processor>();
    }
}