using Godot;

namespace Lavos.Audio
{
    public interface ISoundManager
    {
        /// <summary>
        /// Increase the maximum sources allowed to be created.
        /// If the new maximum is lower than the current, nothing happens.
        /// </summary>
        /// <param name="max">Numberof sources allowed.</param>
        void SetMaximumSources(int max);
        void PlayStream(AudioStreamOGGVorbis stream);
        void PlayStreamUnique(AudioStreamOGGVorbis stream);
    }
}