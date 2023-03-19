using Godot;
using Lavos.Core;
using Lavos.Utils.Automation;
using Lavos.Utils.Extensions;
using System;

namespace Lavos.Audio
{
    internal sealed class FadeOutState : BaseFadeState, IState
    {
        public FadeOutState() : base() { }

        #region IState

        public event EventHandler<IState> StateChanged;

        void IState.Enter()
        {
            _target = _masterAudio.MasterMusicVolume;
            _timer = 0f;
        }

        void IProcessable.Process(double dt)
        {
            _timer += (dt * _musicManager.FadeOutSpeed);
            var weight = 1f - (float)(_timer / Duration);
            _musicManager.Source.SetVolume(Mathf.Lerp(0, _target, weight));
            if (_musicManager.Source.GetVolume() == 0)
            {
                StateChanged?.Invoke(this, null);
            }
        }

        void IState.Exit()
        {
            _musicManager.Source.SetVolume(0);
            _musicManager.Source.Stop();
        }

        #endregion
    }
}