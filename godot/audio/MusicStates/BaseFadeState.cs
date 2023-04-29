using Lavos.Utils.Lazy;

namespace Lavos.Audio;

internal abstract class BaseFadeState
{
    #region Members

    protected float _target = 0;
    protected double _timer = 0;
    protected const double Duration = 0.5;

    protected LazyPin<MasterAudio> _masterAudio;
    protected LazyPin<MusicManager> _musicManager;

    #endregion
}
