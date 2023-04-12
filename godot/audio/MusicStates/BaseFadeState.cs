using Lavos.Scene;

namespace Lavos.Audio;

internal abstract class BaseFadeState
{
    #region Members

    protected float _target = 0;
    protected double _timer = 0;
    protected const double Duration = 0.5;

    protected MasterAudio _masterAudio = null;
    protected MusicManager _musicManager = null;

    #endregion

    internal BaseFadeState()
    {
        _masterAudio = NodeTree.GetPinnedNodeByType<MasterAudio>();
        _musicManager = NodeTree.GetPinnedNodeByType<MusicManager>();
    }
}
