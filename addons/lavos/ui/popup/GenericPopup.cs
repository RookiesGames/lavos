using Godot;
using System;

namespace Lavos.UI;

[Obsolete("Broken. Needs rework")]
public sealed partial class GenericPopup
    : Control
    , IPopup
{
    Label _titleLabel;
    Label _descriptionLabel;

    Button _acceptBtn;
    public Button AcceptButton => _acceptBtn;
    Button _declineBtn;
    public Button DeclineButton => _declineBtn;

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

    public override void _EnterTree()
    {
        _titleLabel = this.GetNodeInTreeByName<Label>("TitleLabel");
        _descriptionLabel = this.GetNodeInTreeByName<Label>("DescriptionLabel");
        _acceptBtn = this.GetNodeInTreeByName<Button>("AcceptButton");
        _declineBtn = this.GetNodeInTreeByName<Button>("DeclineButton");
    }

    public override void _Ready()
    {
        //_acceptBtn.ButtonPressed += OnAccepted;
        //_declineBtn.ButtonPressed += OnDeclined;
    }

    public override void _ExitTree()
    {
        //_acceptBtn.ButtonPressed -= OnAccepted;
        //_declineBtn.ButtonPressed -= OnDeclined;
        //
        PopupResult = null;
    }

    #region IPopup

    public void ShowPopup()
    {
        this.MouseFilter = MouseFilterEnum.Stop;
        Show();
    }

    public void HidePopup()
    {
        Hide();
        this.MouseFilter = MouseFilterEnum.Ignore;
    }

    #endregion IPopup

    void OnAccepted()
    {
        PopupResult?.Invoke(UI.PopupResult.Accepted);
    }

    void OnDeclined()
    {
        PopupResult?.Invoke(UI.PopupResult.Declined);
    }
}
