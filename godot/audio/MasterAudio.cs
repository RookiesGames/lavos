using Godot;
using Lavos.Scene;
using Lavos.Services.Data;
using Lavos.Utils.Extensions;
using System;
using System.Collections.Generic;

namespace Lavos.Audio
{
    public sealed class MasterAudio
        : Node
        , IDataSaver
    {
        public static event Action VolumeChanged;

        Math.Range _masterVolume = new Math.Range(value: 1f, min: 0f, max: 1f);
        public float MasterVolume
        {
            get => _masterVolume.Value;
            set
            {
                _masterVolume.Value = value;
                VolumeChanged?.Invoke();
            }
        }

        Math.Range _musicVolume = new Math.Range(value: 1f, min: 0f, max: 1f);
        public float MasterMusicVolume => _musicVolume.Value * _masterVolume.Value;
        public float MusicVolume
        {
            get => _musicVolume.Value;
            set
            {
                _musicVolume.Value = value;
                VolumeChanged?.Invoke();
            }
        }

        Math.Range _soundVolume = new Math.Range(value: 1f, min: 0f, max: 1f);
        public float MasterSoundVolume => _soundVolume.Value * _masterVolume.Value;
        public float SoundVolume
        {
            get => _soundVolume.Value;
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


        #region IDataSaver

        bool _isDirty = false;
        public bool IsDirty => _isDirty;

        const string dataFile = "masteraudio.dat";
        public string DataFile => dataFile;

        public void WriteData(Dictionary<string, object> data)
        {
            data.SetEntry("master", _masterVolume.Value);
            data.SetEntry("music", _musicVolume.Value);
            data.SetEntry("sound", _soundVolume.Value);
        }

        #endregion
    }
}