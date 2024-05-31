using Godot;

namespace Lavos.UI;

[GlobalClass]
public sealed partial class FadePanelConfig : Resource
{
    [Export] uint _fadeInDuration = 1;
    public uint FadeInDuration => _fadeInDuration;

    [Export] uint _fadeOutDuration = 1;
    public uint FadeOutDuration => _fadeOutDuration;
}
