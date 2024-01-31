using Godot;
using Lavos.Math;

namespace Lavos.Utils.Extensions;

public static class AudioStreamPlayerExtensions
{
    const float OffsetDb = 20;
    const float MaximimDb = 0;
    const float MinimumDb = -1 * OffsetDb;

    readonly static Math.Range _volume = new(0f, 0f, 1f);

    public static void SetVolume(this AudioStreamPlayer player, float value)
    {
        _volume.Value = value;
        var db = Mathf.Lerp(MinimumDb, MaximimDb, _volume.Value);
        player.VolumeDb = (float)(db <= MinimumDb ? double.MinValue : db);
    }

    public static float GetVolume(this AudioStreamPlayer player)
    {
        return (player.VolumeDb + OffsetDb) / OffsetDb;
    }
}
