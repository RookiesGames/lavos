using Godot;
using Lavos.Dependency;
using Lavos.Services.Data;
using System;

namespace Lavos.Audio;

public sealed partial class MasterAudio : Node
{
    AudioDataSaver _dataSaver;

    public static event Action VolumeChanged;

    public float MasterVolume
    {
        get => _dataSaver.MasterVolume;
        set
        {
            _dataSaver.MasterVolume = ClampValue(value);
            VolumeChanged?.Invoke();
        }
    }

    public float MasterMusicVolume => _dataSaver.MusicVolume * _dataSaver.MasterVolume;
    public float MusicVolume
    {
        get => _dataSaver.MusicVolume;
        set
        {
            _dataSaver.MusicVolume = ClampValue(value);
            VolumeChanged?.Invoke();
        }
    }

    public float MasterSoundVolume => _dataSaver.SoundVolume * _dataSaver.MasterVolume;
    public float SoundVolume
    {
        get => _dataSaver.SoundVolume;
        set
        {
            _dataSaver.SoundVolume = ClampValue(value);
            VolumeChanged?.Invoke();
        }
    }

    float ClampValue(float value) => Mathf.Clamp(value, 0f, 1f);

    public override void _Ready()
    {
        var service = ServiceLocator.Locate<IDataSaverService>();
        _dataSaver = service.GetDataSaver<AudioDataSaver>();
        //
        NodeTree.PinNodeByType<MasterAudio>(this);
    }

    public override void _ExitTree()
    {
        NodeTree.UnpinNodeByType<MasterAudio>();
    }
}