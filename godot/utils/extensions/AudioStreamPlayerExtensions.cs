using Godot;
using Lavos.Math;

namespace Lavos.Utils.Extensions
{
    public static class AudioStreamPlayerExtensions
    {
        const float MaximimDb = 0;
        const float MinimumDb = -80;
        const float OffsetDb = 80;

        static Math.Range _volume = new Math.Range(0f, 0f, 1f);


        public static void SetVolume(this AudioStreamPlayer player, float value)
        {
            _volume.Value = value;
            player.VolumeDb = Mathf.Lerp(MinimumDb, MaximimDb, _volume.Value);
        }

        public static float GetVolume(this AudioStreamPlayer player)
        {
            var volume = (player.VolumeDb + OffsetDb) / OffsetDb;
            return volume;
        }
    }
}