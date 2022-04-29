using Godot;
using Lavos.Dependency;
using Lavos.Scene;
using Lavos.Services.Data;
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
                if (_masterVolume.Value == value) { return; }
                //
                _masterVolume.Value = value;
                _isDirty = true;
                //
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
                if (_musicVolume.Value == value) { return; }
                //
                _musicVolume.Value = value;
                _isDirty = true;
                //
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
                if (_soundVolume.Value == value) { return; }
                //
                _soundVolume.Value = value;
                _isDirty = true;
                //
                VolumeChanged?.Invoke();
            }
        }

        public override void _EnterTree()
        {
            NodeTree.PinNode<MasterAudio>(this);
            ServiceLocator
                .Locate<IDataSaverService>()
                .Register(this);
        }

        public override void _ExitTree()
        {
            ServiceLocator
                .Locate<IDataSaverService>()
                .Unregister(this);
            NodeTree.UnpinNode<MasterAudio>();
        }

        #region IDataSaver

        const string MasterKey = "master";
        const string MusicKey = "music";
        const string SoundKey = "sound";

        bool _isDirty = false;
        public bool IsDirty => _isDirty;

        const string dataFile = "masteraudio.dat";
        public string DataFile => dataFile;


        public void LoadData(Dictionary<string, string> data)
        {
            if (data.ContainsKey(MasterKey))
            {
                MasterVolume = float.Parse(data[MasterKey]);
            }
            if (data.ContainsKey(MusicKey))
            {
                MusicVolume = float.Parse(data[MusicKey]);
            }
            if (data.ContainsKey(SoundKey))
            {
                SoundVolume = float.Parse(data[SoundKey]);
            }
        }

        public void WriteData(Dictionary<string, string> data)
        {
            data["master"] = $"{_masterVolume.Value}";
            data["music"] = $"{_musicVolume.Value}";
            data["sound"] = $"{_soundVolume.Value}";
            //
            _isDirty = false;
        }

        #endregion
    }
}