using Godot;
using Lavos.Services.Data;

namespace Lavos.Audio;

[GlobalClass]
public sealed partial class AudioDataSaver : DataSaver
{
    const string MasterVolumeKey = "master_volume_key";
    const string MusicVolumeKey = "music_volume_key";
    const string SoundVolumeKey = "sound_volume_key";

    public override string DataFile => "audio_data";

    public AudioDataSaver()
    {
        // Defaults
        MasterVolume = 1f;
        MusicVolume = 1f;
        SoundVolume = 1f;
    }

    public float MasterVolume
    {
        get => GetData(MasterVolumeKey).AsFloat();
        set => SaveData(MasterVolumeKey, value);
    }
    public float MusicVolume
    {
        get => GetData(MusicVolumeKey).AsFloat();
        set => SaveData(MusicVolumeKey, value);
    }
    public float SoundVolume
    {
        get => GetData(SoundVolumeKey).AsFloat();
        set => SaveData(SoundVolumeKey, value);
    }
}