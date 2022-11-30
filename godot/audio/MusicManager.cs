using Godot;
using Lavos.Scene;
using Lavos.Utils.Automation;
using Lavos.Utils.Extensions;

namespace Lavos.Audio
{
    public sealed partial class MusicManager : Node
    {
        public enum Effect
        {
            Instant,
            FadeIn,
            FadeOut,
        }

        AudioStreamPlayer _source = null;
        MasterAudio _masterAudio = null;

        readonly StateMachine _stateMachine = new StateMachine();
        readonly State _fadeInState = new State();
        readonly State _fadeOutState = new State();
        float _target = 0;
        double _timer = 0;
        const double Duration = 0.5;

        public double FadeInSpeed = 0.25;
        public double FadeOutSpeed = 0.25;


        public override void _EnterTree()
        {
            NodeTree.PinNode<MusicManager>(this);
            MasterAudio.VolumeChanged += OnVolumeChanged;
        }

        public override void _ExitTree()
        {
            MasterAudio.VolumeChanged -= OnVolumeChanged;
            NodeTree.UnpinNode<MusicManager>();
        }

        void OnVolumeChanged()
        {
            _source.SetVolume(_masterAudio.MasterMusicVolume);
        }

        public override void _Ready()
        {
            _source = this.AddNode<AudioStreamPlayer>("MusicSource");
            _masterAudio = NodeTree.GetPinnedNode<MasterAudio>();
            SetupStates();
        }

        void SetupStates()
        {
            _fadeInState.Enter += () =>
            {
                _source.SetVolume(0);
                _source.Play();
                //
                _target = _masterAudio.MasterMusicVolume;
                _timer = 0;
            };
            _fadeInState.Process += (delta) =>
            {
                _timer += (delta * FadeInSpeed);
                var weight = (float)(_timer / Duration);
                _source.SetVolume(Mathf.Lerp(0, _target, weight));
                if (_source.GetVolume() >= _target)
                {
                    _stateMachine.ChangeState(null);
                }
            };
            _fadeInState.Exit += () =>
            {
                _source.SetVolume(_target);
            };
            //
            _fadeOutState.Enter += () =>
            {
                _target = _masterAudio.MasterMusicVolume;
                _timer = 0f;
            };
            _fadeOutState.Process += (delta) =>
            {
                _timer += (delta * FadeOutSpeed);
                var weight = 1f -(float)(_timer / Duration);
                _source.SetVolume(Mathf.Lerp(0, _target, weight));
                if (_source.GetVolume() == 0)
                {
                    _stateMachine.ChangeState(null);
                }
            };
            _fadeOutState.Exit += () =>
            {
                _source.SetVolume(0);
                _source.Stop();
            };
            //
            _stateMachine.ChangeState(null);
        }

        public override void _Process(double delta)
        {
            _stateMachine.Process(delta);
        }

        public void PlayStream(AudioStreamOggVorbis stream, Effect effect = Effect.Instant)
        {
            _source.Stream = stream;
            //
            switch (effect)
            {
                case Effect.Instant: PlayStream(); return;
                case Effect.FadeIn: FadeIn(); return;
                case Effect.FadeOut: FadeOut(); return;
                default: return;
            }
        }

        public void PlayStream()
        {
            _source.SetVolume(_masterAudio.MasterMusicVolume);
            _source.Play();
        }

        public void StopStream()
        {
            _source.Stop();
        }

        public void FadeIn()
        {
            _stateMachine.ChangeState(_fadeInState);
        }

        public void FadeOut()
        {
            _stateMachine.ChangeState(_fadeOutState);
        }
    }
}