using Godot;
using Lavos.Dependency;
using Lavos.Console;
using Lavos.Utils.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lavos.Audio
{
    sealed class SoundManager
        : Node
        , ISoundManager
    {
        const string Tag = nameof(SoundManager);
        int _simultaniousSounds = 32;
        List<AudioStreamPlayer> _sources = new List<AudioStreamPlayer>();
        int _nextAvailable = 0;


        public override void _Ready()
        {
            CreateSource();
        }

        public override void _ExitTree()
        {
            foreach (var source in _sources)
            {
                source.RemoveSelf();
            }
            _sources.Clear();
        }

        #region ISoundManager

        public void SetMaximumSources(int max)
        {
            if (max > _simultaniousSounds)
            {
                _simultaniousSounds = max;
            }
        }

        public void PlayStream(AudioStreamOGGVorbis stream)
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

        AudioStreamPlayer CreateSource()
        {
            _nextAvailable = _sources.Count;
            var source = this.AddNode<AudioStreamPlayer>($"SoundSource{_sources.Count}");
            _sources.Add(source);
            return source;
        }

        public void PlayStreamUnique(AudioStreamOGGVorbis stream)
        {
            if (IsStreamPlaying(stream) == false)
            {
                PlayStream(stream);
            }
        }

        bool IsStreamPlaying(AudioStreamOGGVorbis stream)
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

        #endregion
    }
}