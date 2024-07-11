using Godot;
using Lavos.Dependency;
using Lavos.Nodes;
using Lavos.Services.Data;

namespace Lavos.Audio;

sealed partial class AudioConfig : DataSaverConfig
{
    public override void Initialize(IDependencyResolver resolver)
    {
        base.Initialize(resolver);
        //
        var audio = OmniNode.Instance.AddNode<Node>("Audio");
        audio.AddNode<MasterAudio>();
        audio.AddNode<MusicManager>();
        audio.AddNode<SoundManager>();
    }
}