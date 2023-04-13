using Godot;
using System;


namespace Lavos.Scene;

public sealed partial class SceneLoader : Node
{
    [Export] PackedScene _nextScene = null;
    [Export] double _delay = 0;

    double _currentTime;


    #region Node

    public override void _Process(double delta)
    {
        _currentTime += delta;
        if (_currentTime >= _delay)
        {
            SceneManager.ChangeScene(_nextScene);
        }
    }

    #endregion
}
