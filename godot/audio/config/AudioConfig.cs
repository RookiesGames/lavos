using Godot;
using Lavos.Dependency;
using Lavos.Nodes;

namespace Lavos.Audio;

sealed partial class AudioConfig : Config
{
    public override void Configure(IDependencyBinder binder) { }
    public override void Initialize(IDependencyResolver resolver)
    {
        var audio = OmniNode.Instance.AddNode<Node>("Audio");
        audio.AddNode<MasterAudio>();
        audio.AddNode<MusicManager>();
        audio.AddNode<SoundManager>();
    }
}