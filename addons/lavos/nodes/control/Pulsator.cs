using Godot;
using Lavos.Utils.Automation;

namespace Lavos.Nodes.Controls;

[Tool]
public sealed partial class Pulsator : Control
{
    [Export] float Duration = 5f;
    [Export] float PulseOffset = 0.5f;

    Vector2 _baseScale = Vector2.One;
    Vector2 _targetScale = Vector2.Zero;
    float _timer = 0;
    bool _pulsingIn = true;

    public override void _Ready()
    {
        _baseScale = this.Scale - new Vector2(PulseOffset, PulseOffset);
    }

    public override void _Process(double delta)
    {
        switch (_pulsingIn)
        {
            case true: PulseIn(delta); break;
            case false: PulseOut(delta); break;
        }
        this.Scale = _targetScale;
    }

    void PulseIn(double delta)
    {
        _timer += (float)delta;
        _timer = Mathf.Min(_timer, Duration);
        _targetScale.X = _targetScale.Y = (float)Mathf.Lerp(_baseScale.X - PulseOffset, _baseScale.X + PulseOffset, _timer / Duration);
        _pulsingIn = _timer < Duration;
    }
    void PulseOut(double delta)
    {
        _timer -= (float)delta;
        _timer = Mathf.Max(_timer, 0);
        _targetScale.X = _targetScale.Y = (float)Mathf.Lerp(_baseScale.X - PulseOffset, _baseScale.X + PulseOffset, _timer / Duration);
        _pulsingIn = _timer <= 0;

    }
}