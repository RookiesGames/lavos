using Godot;
using Lavos.Services.Data;

namespace Lavos.Audio;

[GlobalClass]
public sealed partial class AudioDataSaver : DataSaver
{
    const string MasterVolumeKey = "master_volume_key";
    const string MusicVolumeKey = "music_volume_key";
    const string SoundVolumeKey = "sound_volume_key";

    public override string DataFile => "audio_data.dat";

    public float MasterVolume
    {
        get => Data.GetOrDefault(MasterVolumeKey, Variant.CreateFrom(1f)).AsFloat();
        set => SaveData(MasterVolumeKey, value);
    }
    public float MusicVolume
    {
        get => Data.GetOrDefault(MusicVolumeKey, Variant.CreateFrom(1f)).AsFloat();
        set => SaveData(MusicVolumeKey, value);
    }
    public float SoundVolume
    {
        get => Data.GetOrDefault(SoundVolumeKey, Variant.CreateFrom(1f)).AsFloat();
        set => SaveData(SoundVolumeKey, value);
    }
}