using Lavos.Utils.Automation;

namespace Lavos.UI
{
    internal abstract class BaseFadeState
    {
        #region Members

        protected double _timer = 0;
        protected readonly double _duration = 0.0;
        protected readonly FadePanel _panel;

        #endregion

        protected BaseFadeState(FadePanel panel, double duration)
        {
            _panel = panel;
            _duration = duration;
        }
    }
}