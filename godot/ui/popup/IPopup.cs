using System;

namespace Lavos.UI;

public interface IPopup
{
    string TitleText { get; set; }
    string DescriptionText { get; set; }
    string AcceptText { get; set; }
    string DeclineText { get; set; }

    event Action<PopupResult> PopupResult;

    void ShowPopup();
    void HidePopup();
}
