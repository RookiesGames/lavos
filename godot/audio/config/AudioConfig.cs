using Godot;
using Lavos.Dependency;
using Lavos.Nodes;
using Lavos.Utils.Extensions;

namespace Lavos.Audio
{
    sealed class AudioConfig : Config
    {
        public override void Configure(IDependencyBinder binder)
        {
            var audio = OmniNode.Instance.AddNode<Node>("Audio");
            audio.AddNode<MasterAudio>();
            audio.AddNode<MusicManager>();
            audio.AddNode<SoundManager>();
        }

        public override void Initialize(IDependencyResolver resolver) { }
    }
}