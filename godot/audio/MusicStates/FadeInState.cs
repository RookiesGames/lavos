using Godot;
using Lavos.Core;
using Lavos.Utils.Automation;
using System;

namespace Lavos.Audio;

internal sealed class FadeInState : BaseFadeState, IState
{
    public FadeInState() : base() { }

    #region IState

    public event EventHandler<IState> StateChanged;

    void IState.Enter()
    {
        _musicManager.Source.SetVolume(0);
        _musicManager.Source.Play();
        //
        _target = _masterAudio.MasterMusicVolume;
        _timer = 0;
    }

    void IProcessable.Process(double delta)
    {
        _timer += (delta * _musicManager.FadeInSpeed);
        var weight = (float)(_timer / Duration);
        _musicManager.Source.SetVolume(Mathf.Lerp(0, _target, weight));
        if (_musicManager.Source.GetVolume() >= _target)
        {
            StateChanged?.Invoke(this, null);
        }
    }

    void IState.Exit()
    {
        _musicManager.Source.SetVolume(_target);
    }

    #endregion
}
