using Godot;
using Lavos.Debug;
using Lavos.Utils.Extensions;

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

        #endregion


        public override void _EnterTree()
        {
            _titleLabel = this.GetNodeInChildrenByName<Label>("TitleLabel");
            _descriptionLabel = this.GetNodeInChildrenByName<Label>("DescriptionLabel");
            _acceptBtn = this.GetNodeInChildrenByName<Button>("AcceptButton");
            _declineBtn = this.GetNodeInChildrenByName<Button>("DeclineButton");
            //
            Assert.IsTrue(_titleLabel != null, "Failed to find title label");
            Assert.IsTrue(_descriptionLabel != null, "Failed to find description label");
            Assert.IsTrue(_acceptBtn != null, "Failed to find accept button");
            Assert.IsTrue(_declineBtn != null, "Failed to find decline button");
        }

        #region IPopup

        public void ShowPopup() { }
        public void HidePopup() { }

        #endregion IPopup
    }
}