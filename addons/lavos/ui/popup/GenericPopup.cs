using Godot;
using System;

namespace Lavos.UI;

public sealed partial class GenericPopup
    : Control
    , IPopup
{
    [Export] Label _titleLabel;
    [Export] Label _descriptionLabel;

    [Export] Button _acceptBtn;
    [Export] Button _declineBtn;

    #region IPopup

    public string TitleText
    {
        get => _titleLabel.Text;
        set => _titleLabel.Text = value;
    }

    public string DescriptionText
    {
        get => _descriptionLabel.Text;
        set => _descriptionLabel.Text = value;
    }

    public string AcceptText
    {
        get => _acceptBtn.Text;
        set => _acceptBtn.Text = value;
    }

    public string DeclineText
    {
        get => _declineBtn.Text;
        set => _declineBtn.Text = value;
    }

    public event Action<PopupResult> PopupResult;

    #endregion

    public override void _Ready()
    {
        _acceptBtn.Pressed += OnAccepted;
        _declineBtn.Pressed += OnDeclined;
    }

    public override void _ExitTree()
    {
        _acceptBtn.Pressed -= OnAccepted;
        _declineBtn.Pressed -= OnDeclined;
        //
        PopupResult = null;
    }

    #region IPopup

    public void ShowPopup()
    {
        MouseFilter = MouseFilterEnum.Stop;
        Show();
    }

    public void HidePopup()
    {
        Hide();
        MouseFilter = MouseFilterEnum.Ignore;
    }

    #endregion IPopup

    void OnAccepted()
    {
        PopupResult?.Invoke(UI.PopupResult.Accepted);
    }

    public void Decline() => OnDeclined();
    void OnDeclined()
    {
        PopupResult?.Invoke(UI.PopupResult.Declined);
    }
}
