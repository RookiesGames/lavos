using Godot;

namespace Lavos.Audio
{
    public interface IMusicManager
    {
        void PlayStream(AudioStreamOGGVorbis stream);
    }
}