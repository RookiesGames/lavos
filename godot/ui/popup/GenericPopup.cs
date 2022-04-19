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

        ClickButton _acceptBtn = null;
        public ClickButton AcceptButton => _acceptBtn;
        ClickButton _declineBtn = null;
        public ClickButton DeclineButton => _declineBtn;


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
            _titleLabel = this.GetNodeInChildrenByName<Label>("TitleLabel");
            _descriptionLabel = this.GetNodeInChildrenByName<Label>("DescriptionLabel");
            _acceptBtn = this.GetNodeInChildrenByName<ClickButton>("AcceptButton");
            _declineBtn = this.GetNodeInChildrenByName<ClickButton>("DeclineButton");
        }

        public override void _Ready()
        {
            _acceptBtn.ButtonPressed += OnAccepted;
            _declineBtn.ButtonPressed += OnDeclined;
        }

        public override void _ExitTree()
        {
            _acceptBtn.ButtonPressed -= OnAccepted;
            _declineBtn.ButtonPressed -= OnDeclined;
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
}