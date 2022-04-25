using Godot;
using System;


namespace Lavos.Scene
{
    public sealed class SceneLoader : Node
    {
        [Export] PackedScene _nextScene = null;
        [Export] float _delay = 0f;

        private float _currentTime;


        #region Node

        public override void _Process(float delta)
        {
            _currentTime += delta;
            if (_currentTime >= _delay)
            {
                SceneManager.ChangeScene(_nextScene);
            }
        }

        #endregion
    }
}