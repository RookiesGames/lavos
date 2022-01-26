using Godot;
using System;


namespace Lavos.Core.Scene
{
    public class SceneLoader : Node
    {
        [Export] PackedScene _nextScene;
        [Export] float _delay;

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