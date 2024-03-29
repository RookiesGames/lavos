using Godot;

namespace Lavos.Nodes.Controls;

[Tool]
public sealed partial class Rotator : Control
{
    [Export] float RevolutionTime = 5;

    float _timer = 0;
    float _rotationFactor = 1f;
    public bool Paused
    {
        get => _rotationFactor == 0f;
        set => _rotationFactor = value ? 0f : 1f;
    }

    public override void _Process(double delta)
    {
        _timer += (float)(delta * _rotationFactor);
        _timer = Mathf.Min(_timer, RevolutionTime);
        this.Rotation = Mathf.Lerp(0, 2 * Mathf.Pi, _timer / RevolutionTime);
        if (_timer >= RevolutionTime)
        {
            _timer -= RevolutionTime;
        }
    }
}
