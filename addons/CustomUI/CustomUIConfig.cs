using Godot;
using Lavos.Dependency;

namespace Lavos.UI
{
    public sealed class CustomUIConfig : Config
    {
        [Export] AudioStreamOGGVorbis AcceptSound = null;
        [Export] AudioStreamOGGVorbis CancelSound = null;


        public override void Configure(IDependencyBinder binder)
        {
            ClickButton.AcceptSound = AcceptSound;
            ClickButton.CancelSound = CancelSound;
        }

        public override void Initialize(IDependencyResolver resolver) { }
    }
}