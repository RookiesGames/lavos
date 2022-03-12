using Godot;
using Lavos.Core.Dependency;
using Lavos.Core.Console;
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
        List<AudioStreamPlayer2D> _sources = new List<AudioStreamPlayer2D>();
        int _nextAvailable = 0;

        public override void _Ready()
        {
            ServiceLocator.Register<ISoundManager, SoundManager>(this);
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

        public async Task PlayStream(AudioStreamOGGVorbis stream)
        {
            var source = FindSource();
            source.Stream = stream;
            source.Play();
            await Task.Delay(System.TimeSpan.FromSeconds(stream.GetLength()));
        }

        AudioStreamPlayer2D FindSource()
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

        AudioStreamPlayer2D CreateSource()
        {
            _nextAvailable = _sources.Count;
            var source = this.AddNode<AudioStreamPlayer2D>($"SoundSource{_sources.Count}");
            _sources.Add(source);
            return source;
        }

        #endregion
    }
}