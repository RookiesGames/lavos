using Godot;

namespace Lavos.UI;

[GlobalClass]
public sealed partial class FadePanelResource : Resource
{
    [Export] bool _faded;
    public bool Faded => _faded;

    [Export] double _fadeInDuration = 1;
    public double FadeInDuration => _fadeInDuration;

    [Export] double _fadeOutDuration = 1;
    public double FadeOutDuration => _fadeOutDuration;
}
