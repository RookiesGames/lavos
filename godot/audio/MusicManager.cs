using Godot;
using Lavos.Scene;
using Lavos.Utils.Automation;

namespace Lavos.Audio;

public sealed partial class MusicManager : Node
{
    public enum Effect
    {
        Instant,
        FadeIn,
        FadeOut,
    }

    AudioStreamPlayer _source = null;
    public AudioStreamPlayer Source => _source;

    MasterAudio _masterAudio = null;

    readonly IStateMachine _stateMachine = new StateMachine();
    readonly IState _fadeInState = new FadeInState();
    readonly IState _fadeOutState = new FadeOutState();
    public double FadeInSpeed = 0.25;
    public double FadeOutSpeed = 0.25;

    public override void _EnterTree()
    {
        NodeTree.PinNodeByType<MusicManager>(this);
        MasterAudio.VolumeChanged += OnVolumeChanged;
    }

    public override void _ExitTree()
    {
        MasterAudio.VolumeChanged -= OnVolumeChanged;
        NodeTree.UnpinNodeByType<MusicManager>();
    }

    void OnVolumeChanged()
    {
        _source.SetVolume(_masterAudio.MasterMusicVolume);
    }

    public override void _Ready()
    {
        _source = this.AddNode<AudioStreamPlayer>("MusicSource");
        _masterAudio = NodeTree.GetPinnedNodeByType<MasterAudio>();
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