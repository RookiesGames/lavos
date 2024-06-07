using Godot;

namespace Lavos.Audio;

internal sealed class FadeOutState : BaseFadeState
{
    #region State

    public override void Enter()
    {
        _target = _masterAudio.Pin.MasterMusicVolume;
        _timer = 0f;
    }

    public override void Update(double delta)
    {
        _timer += (delta * _musicManager.Pin.FadeOutSpeed);
        var weight = 1f - (float)(_timer / Duration);
        _musicManager.Pin.Source.SetVolume(Mathf.Lerp(0, _target, weight));
        if (_musicManager.Pin.Source.GetVolume() == 0)
        {
            StateMachine.GoToState<FadeIdleState>();
        }
    }

    public override void Exit()
    {
        _musicManager.Pin.Source.SetVolume(0);
        _musicManager.Pin.Source.Stop();
    }

    #endregion
}