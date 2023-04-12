using Godot;
using Lavos.Scene;
using System.Collections.Generic;

namespace Lavos.Audio;

public sealed partial class SoundManager : Node
{
    const string Tag = nameof(SoundManager);

    MasterAudio _masterAudio = null;
    List<AudioStreamPlayer> _sources = new List<AudioStreamPlayer>();
    int _simultaniousSounds = 32;
    int _nextAvailable = 0;


    public override void _EnterTree()
    {
        NodeTree.PinNodeByType<SoundManager>(this);
        MasterAudio.VolumeChanged += OnVolumeChanged;
    }

    public override void _ExitTree()
    {
        MasterAudio.VolumeChanged -= OnVolumeChanged;
        ClearSources();
        NodeTree.UnpinNodeByType<SoundManager>();
    }

    void OnVolumeChanged()
    {
        foreach (var source in _sources)
        {
            source.SetVolume(_masterAudio.MasterSoundVolume);
        }
    }

    public override void _Ready()
    {
        _masterAudio = NodeTree.GetPinnedNodeByType<MasterAudio>();
        CreateSource();
    }

    AudioStreamPlayer CreateSource()
    {
        _nextAvailable = _sources.Count;
        var source = this.AddNode<AudioStreamPlayer>($"SoundSource{_sources.Count}");
        source.SetVolume(_masterAudio.MasterSoundVolume);
        _sources.Add(source);
        return source;
    }

    void ClearSources()
    {
        foreach (var source in _sources)
        {
            source.Stop();
            source.RemoveSelf();
        }
        _sources.Clear();
    }

    public void SetMaximumSources(int max)
    {
        if (max > _simultaniousSounds)
        {
            _simultaniousSounds = max;
        }
    }

    public void PlayStream(AudioStreamOggVorbis stream)
    {
        var source = FindSource();
        source.Stream = stream;
        source.Play();
    }

    AudioStreamPlayer FindSource()
    {
        for (var idx = 0; idx < _sources.Count; ++idx)
        {
            if (_sources[_nextAvailable].Playing == false)
            {
                return _sources[_nextAvailable];
            }

            _nextAvailable = (++_nextAvailable) % _sources.Count;
        }

        if (_sources.Count < _simultaniousSounds)
        {
            return CreateSource();
        }

        Log.Error(Tag, "All sound sources were used up");
        return null;
    }

    public void PlayStreamUnique(AudioStreamOggVorbis stream)
    {
        if (IsStreamPlaying(stream) == false)
        {
            PlayStream(stream);
        }
    }

    bool IsStreamPlaying(AudioStreamOggVorbis stream)
    {
        foreach (var source in _sources)
        {
            if (source.Playing && source.Stream == stream)
            {
                return true;
            }
        }
        //
        return false;
    }

    public void StopAll()
    {
        foreach (var source in _sources)
        {
            source.Stop();
        }
    }
}