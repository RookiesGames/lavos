using Lavos.Core;

namespace Lavos.Audio;

internal abstract class BaseFadeState
{
    #region Members

    protected float _target = 0;
    protected double _timer = 0;

    public double Duration = 0.25;

    protected LazyPin<MasterAudio> _masterAudio;
    protected LazyPin<MusicManager> _musicManager;

    #endregion
}
