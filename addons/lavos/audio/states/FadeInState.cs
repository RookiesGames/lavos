using Godot;
using Lavos.Core;
using Lavos.Utils.Automation;
using System;

namespace Lavos.Audio;

internal sealed class FadeInState : BaseFadeState
{
    public FadeInState(double duration) : base()
    {
        Duration = duration;
    }

    #region State

    public override void Enter()
    {
        _musicManager.Pin.Source.SetVolume(0);
        _musicManager.Pin.Source.Play();
        //
        _target = _masterAudio.Pin.MasterMusicVolume;
        _timer = 0;
    }

    public override void Update(double delta)
    {
        _timer += (delta * _musicManager.Pin.FadeInSpeed);
        var weight = (float)(_timer / Duration);
        _musicManager.Pin.Source.SetVolume(Mathf.Lerp(0, _target, weight));
        if (_musicManager.Pin.Source.GetVolume() >= _target)
        {
            StateMachine.ChangeState(null);
        }
    }

    public override void Exit()
    {
        _musicManager.Pin.Source.SetVolume(_target);
    }

    #endregion
}
