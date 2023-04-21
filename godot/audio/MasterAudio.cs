using Godot;
using System;

namespace Lavos.Audio;

public sealed partial class MasterAudio : Node
{
    public static event Action VolumeChanged;

    readonly Math.Range _masterVolume = new(value: 1f, min: 0f, max: 1f);
    public float MasterVolume
    {
        get => _masterVolume.Value;
        set
        {
            if (_masterVolume.Value == value) { return; }
            //
            _masterVolume.Value = value;
            VolumeChanged?.Invoke();
        }
    }

    readonly Math.Range _musicVolume = new(value: 1f, min: 0f, max: 1f);
    public float MasterMusicVolume => _musicVolume.Value * _masterVolume.Value;
    public float MusicVolume
    {
        get => _musicVolume.Value;
        set
        {
            if (_musicVolume.Value == value) { return; }
            //
            _musicVolume.Value = value;
            VolumeChanged?.Invoke();
        }
    }

    readonly Math.Range _soundVolume = new(value: 1f, min: 0f, max: 1f);
    public float MasterSoundVolume => _soundVolume.Value * _masterVolume.Value;
    public float SoundVolume
    {
        get => _soundVolume.Value;
        set
        {
            if (_soundVolume.Value == value) { return; }
            //
            _soundVolume.Value = value;
            VolumeChanged?.Invoke();
        }
    }

    public override void _EnterTree()
    {
        NodeTree.PinNodeByType<MasterAudio>(this);
    }

    public override void _ExitTree()
    {
        NodeTree.UnpinNodeByType<MasterAudio>();
    }
}