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

    readonly IStateMachine _stateMachine = new StateMachine(null);
    public double FadeInSpeed = 1;
    public double FadeOutSpeed = 1;
    const double DefaultFadeDuration = 1;

    public override void _EnterTree()
    {
        NodeTree.PinNodeByType<MusicManager>(this);
        MasterAudio.VolumeChanged += OnVolumeChanged;
    }

    public override void _ExitTree()
    {
        MasterAudio.VolumeChanged -= OnVolumeChanged;
        NodeTree.UnpinNodeByType<MusicManager>();
        //
        _stateMachine.Dispose();
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

    public Task PlayStream(AudioStream stream, Effect effect = Effect.Instant)
    {
        _source.Stream = stream;
        //
        switch (effect)
        {
            case Effect.Instant: PlayStream(); return Task.CompletedTask;
            case Effect.FadeIn: return FadeIn();
            case Effect.FadeOut: return FadeOut();
            default: return Task.CompletedTask;
        }
    }

    public void SetStream(AudioStream stream) => _source.Stream = stream;
    public void PlayStream() => _source.Play();
    public void StopStream() => _source.Stop();
    public void ResumeStream() => _source.StreamPaused = false;
    public void PauseStream() => _source.StreamPaused = true;

    public async Task FadeInAsync(double duration = DefaultFadeDuration)
    {
        var state = new FadeInState(duration);
        _stateMachine.ChangeState(state);
        await Task.Delay((int)(duration * 1000));
    }

    public Task FadeIn(double duration = DefaultFadeDuration)
    {
        var state = new FadeInState(duration);
        _stateMachine.ChangeState(state);
        return Task.CompletedTask;
    }

    public async Task FadeOutAsync(double duration = DefaultFadeDuration)
    {
        var state = new FadeOutState(duration);
        _stateMachine.ChangeState(state);
        await Task.Delay((int)(duration * 1000));
    }

    public Task FadeOut(double duration = DefaultFadeDuration)
    {
        var state = new FadeOutState(duration);
        _stateMachine.ChangeState(state);
        return Task.CompletedTask;
    }

    public async Task FadeOutAndIn(AudioStream stream, double fadeOutDuration = DefaultFadeDuration, double fadeInDuration = DefaultFadeDuration)
    {
        await FadeOut(fadeOutDuration);
        SetStream(stream);
        await FadeInAsync(fadeInDuration);
    }
}