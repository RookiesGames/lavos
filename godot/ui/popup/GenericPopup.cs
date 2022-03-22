using Godot;
using Lavos.Debug;
using Lavos.Utils.Extensions;
using System;

namespace Lavos.UI
{
    public class GenericPopup
        : Control
        , IPopup
    {
        Label _titleLabel = null;
        Label _descriptionLabel = null;

        Button _acceptBtn = null;
        Button _declineBtn = null;

        ColorRect _background;

        Action<PopupResult> _popupResult = null;


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

        public Action<PopupResult> PopupResult
        {
            get => _popupResult;
            set => _popupResult = value;
        }

        #endregion


        public override void _EnterTree()
        {
            _titleLabel = this.GetNodeInChildrenByName<Label>("TitleLabel");
            _descriptionLabel = this.GetNodeInChildrenByName<Label>("DescriptionLabel");
            _acceptBtn = this.GetNodeInChildrenByName<Button>("AcceptButton");
            _declineBtn = this.GetNodeInChildrenByName<Button>("DeclineButton");
        }

        public override void _ExitTree()
        {
            _popupResult = null;
        }

        #region IPopup

        public void ShowPopup()
        {
            this.MouseFilter = MouseFilterEnum.Stop;
            this.Visible = true;
        }

        public void HidePopup()
        {
            this.Visible = false;
            this.MouseFilter = MouseFilterEnum.Ignore;
        }

        #endregion IPopup

        public void OnPopupAccepted()
        {
            PopupResult?.Invoke(UI.PopupResult.Accepted);
        }

        public void OnPopupDeclied()
        {
            PopupResult?.Invoke(UI.PopupResult.Declined);
        }
    }
}