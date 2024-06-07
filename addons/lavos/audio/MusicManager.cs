using Godot;
using Lavos.Utils.Automation;
using System.Threading.Tasks;

namespace Lavos.Audio;

public sealed partial class MusicManager : Node
{
    public enum Effect
    {
        Instant,
        FadeIn,
        FadeOut,
    }

    AudioStreamPlayer _source;
    public AudioStreamPlayer Source => _source;

    MasterAudio _masterAudio;

    readonly PersistentStateMachine _stateMachine = new();
    public double FadeInSpeed = 1;
    public double FadeOutSpeed = 1;
    const double DefaultFadeDuration = 1;

    public override void _EnterTree()
    {
        _stateMachine.AddState<FadeIdleState>();
        _stateMachine.AddState<FadeInState>();
        _stateMachine.AddState<FadeOutState>();
        _stateMachine.GoToState<FadeIdleState>();
        //
        NodeTree.PinNodeByType<MusicManager>(this);
        MasterAudio.VolumeChanged += OnVolumeChanged;
    }

    public override void _ExitTree()
    {
        MasterAudio.VolumeChanged -= OnVolumeChanged;
        NodeTree.UnpinNodeByType<MusicManager>();
    }

    public override void _Process(double delta)
    {
        _stateMachine.Process(delta);
    }

    void OnVolumeChanged()
    {
        _source.SetVolume(_masterAudio.MasterMusicVolume);
    }

    public override void _Ready()
    {
        _source = this.AddNode<AudioStreamPlayer>("MusicSource");
        _masterAudio = NodeTree.GetPinnedNodeByType<MasterAudio>();
        OnVolumeChanged();
    }

    public void PlayStream(AudioStream stream, Effect effect = Effect.Instant)
    {
        _source.Stream = stream;
        //
        switch (effect)
        {
            case Effect.Instant: PlayStream(); break;
            case Effect.FadeIn: FadeIn(); break;
            case Effect.FadeOut: FadeOut(); break;
            default: break;
        }
    }

    public void SetStream(AudioStream stream) => _source.Stream = stream;
    public void PlayStream() => _source.Play();
    public void StopStream() => _source.Stop();
    public void ResumeStream() => _source.StreamPaused = false;
    public void PauseStream() => _source.StreamPaused = true;

    public async Task FadeInAsync(double duration = DefaultFadeDuration)
    {
        _stateMachine.GetState<FadeInState>().Duration = duration;
        _stateMachine.GoToState<FadeInState>();
        await Task.Delay((int)(duration * 1000));
    }

    public void FadeIn(double duration = DefaultFadeDuration)
    {
        _stateMachine.GetState<FadeInState>().Duration = duration;
        _stateMachine.GoToState<FadeInState>();
    }

    public async Task FadeOutAsync(double duration = DefaultFadeDuration)
    {
        _stateMachine.GetState<FadeOutState>().Duration = duration;
        _stateMachine.GoToState<FadeOutState>();
        await Task.Delay((int)(duration * 1000));
    }

    public void FadeOut(double duration = DefaultFadeDuration)
    {
        _stateMachine.GetState<FadeOutState>().Duration = duration;
        _stateMachine.GoToState<FadeOutState>();
    }

    public async Task FadeOutAndIn(AudioStream stream, double fadeOutDuration = DefaultFadeDuration, double fadeInDuration = DefaultFadeDuration)
    {
        await FadeOutAsync(fadeOutDuration);
        SetStream(stream);
        await FadeInAsync(fadeInDuration);
    }
}