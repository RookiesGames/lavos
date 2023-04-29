using Godot;
using Lavos.Core;
using Lavos.Utils.Automation;
using System;

namespace Lavos.Audio;

internal sealed class FadeOutState : BaseFadeState, IState
{
    public FadeOutState() : base() { }

    #region IState

    public event EventHandler<IState> StateChanged;

    void IState.Enter()
    {
        _target = _masterAudio.Pin.MasterMusicVolume;
        _timer = 0f;
    }

    void IProcessable.Process(double delta)
    {
        _timer += (delta * _musicManager.Pin.FadeOutSpeed);
        var weight = 1f - (float)(_timer / Duration);
        _musicManager.Pin.Source.SetVolume(Mathf.Lerp(0, _target, weight));
        if (_musicManager.Pin.Source.GetVolume() == 0)
        {
            StateChanged?.Invoke(this, null);
        }
    }

    void IState.Exit()
    {
        _musicManager.Pin.Source.SetVolume(0);
        _musicManager.Pin.Source.Stop();
    }

    #endregion
}