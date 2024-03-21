using Godot;
using System.Collections.Generic;

namespace Lavos.Audio;

public sealed partial class SoundManager : Node
{
    const string Tag = nameof(SoundManager);

    MasterAudio _masterAudio;
    HashSet<AudioStreamPlayer> _sources;
    int _simultaniousSounds = 32;

    public override void _EnterTree()
    {
        _sources = new();
    }

    public override void _Ready()
    {
        _masterAudio = NodeTree.GetPinnedNodeByType<MasterAudio>();
        CreateSource();

        NodeTree.PinNodeByType<SoundManager>(this);
        MasterAudio.VolumeChanged += OnVolumeChanged;
    }

    AudioStreamPlayer CreateSource()
    {
        var source = this.AddNode<AudioStreamPlayer>($"SoundSource{_sources.Count}");
        source.SetVolume(_masterAudio.MasterSoundVolume);
        _sources.Add(source);
        return source;
    }

    void OnVolumeChanged()
    {
        foreach (var source in _sources)
        {
            source.SetVolume(_masterAudio.MasterSoundVolume);
        }
    }

    public override void _ExitTree()
    {
        MasterAudio.VolumeChanged -= OnVolumeChanged;
        ClearSources();
        NodeTree.UnpinNodeByType<SoundManager>();
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
        foreach (var source in _sources)
        {
            if (source.Playing == false)
            {
                return source;
            }
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
        if (!IsStreamPlaying(stream))
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