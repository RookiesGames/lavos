using Godot;
using Lavos.Core;
using Lavos.Utils.Automation;
using System;

namespace Lavos.Audio;

internal sealed class FadeInState : BaseFadeState, IState
{
    public FadeInState() : base() { }

    #region IState

    public event Action<IState> StateChanged;

    void IState.Enter()
    {
        _musicManager.Pin.Source.SetVolume(0);
        _musicManager.Pin.Source.Play();
        //
        _target = _masterAudio.Pin.MasterMusicVolume;
        _timer = 0;
    }

    void IProcessable.Process(double delta)
    {
        _timer += (delta * _musicManager.Pin.FadeInSpeed);
        var weight = (float)(_timer / Duration);
        _musicManager.Pin.Source.SetVolume(Mathf.Lerp(0, _target, weight));
        if (_musicManager.Pin.Source.GetVolume() >= _target)
        {
            StateChanged?.Invoke(null);
        }
    }

    void IState.Exit()
    {
        _musicManager.Pin.Source.SetVolume(_target);
    }

    #endregion
}
