using Godot;
using Lavos.Scene;
using System;

namespace Lavos.Audio
{
    public class MasterAudio : Node
    {
        public static event Action VolumeChanged;

        Math.Range _masterVolume = new Math.Range(1f, 0f, 1f);
        public float MasterVolume
        {
            get => _masterVolume.Value;
            set
            {
                _masterVolume.Value = value;
                VolumeChanged?.Invoke();
            }
        }

        Math.Range _musicVolume = new Math.Range(1f, 0f, 1f);
        public float MusicVolume
        {
            get => _musicVolume.Value * _masterVolume.Value;
            set
            {
                _musicVolume.Value = value;
                VolumeChanged?.Invoke();
            }
        }

        Math.Range _soundVolume = new Math.Range(1f, 0f, 1f);
        public float SoundVolume
        {
            get => _soundVolume.Value * _masterVolume.Value;
            set
            {
                _soundVolume.Value = value;
                VolumeChanged?.Invoke();
            }
        }

        public override void _EnterTree()
        {
            NodeTree.PinNode<MasterAudio>(this);
        }

        public override void _ExitTree()
        {
            NodeTree.UnpinNode<MasterAudio>();
        }
    }
}