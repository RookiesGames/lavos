using Godot;
using Lavos.Utils.Extensions;

namespace Lavos.Audio
{
    public sealed class MusicManager
        : Node
        , IMusicManager
    {
        AudioStreamPlayer _source = null;

        public override void _Ready()
        {
            _source = this.AddNode<AudioStreamPlayer>("MusicSource");
        }

        public void PlayStream(AudioStreamOGGVorbis stream)
        {
            _source.Stream = stream;
            _source.Play();
        }
    }
}