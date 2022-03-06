using Godot;
using System.Threading.Tasks;

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
        Task PlayStream(AudioStreamOGGVorbis stream);
    }
}