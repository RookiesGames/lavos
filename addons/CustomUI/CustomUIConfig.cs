using Godot;
using Lavos.Dependency;

namespace Lavos.UI;

public sealed partial class CustomUIConfig : Config
{
    [Export] AudioStreamOggVorbis AcceptSound = null;
    [Export] AudioStreamOggVorbis CancelSound = null;


    public override void Configure(IDependencyBinder binder)
    {
        ClickButton.AcceptSound = AcceptSound;
        ClickButton.CancelSound = CancelSound;
    }

    public override void Initialize(IDependencyResolver resolver) { }
}