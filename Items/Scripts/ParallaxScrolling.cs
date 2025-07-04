using Godot;
using System;

namespace UnicornGame
{
    public partial class ParallaxScrolling : Parallax2D
    {
        private Player _playerCharacter;
        private Vector2 _previousPlayerPosition;
        private Vector2 _currentPlayerPosition;
        private Vector2 _movementAmount;
        private Vector2 _parallaxMagnitude;
        private Vector2 _resetPosition;
        public override void _Ready()
        {
            _playerCharacter = GetNode<Player>("/root/Level1/PlayerCharacter");
            if (_playerCharacter != null)
            {
                GD.Print("Got PlayerCharacter reference successfully");
                _currentPlayerPosition = _playerCharacter.GlobalPosition;
            }
            else
            {
                GD.Print("PlayerCharacter is null");
            }
            _parallaxMagnitude = new Vector2(0.2f, 0f);
            _previousPlayerPosition = Vector2.Zero;
            _resetPosition = GlobalPosition;
        }
        public override void _PhysicsProcess(double delta)
        {
            GetPlayerPosition();
        }


        public Vector2 GetPlayerPosition()
        {
            GD.Print($"Player global position: {_playerCharacter.GlobalPosition}");
            _currentPlayerPosition = _playerCharacter.GlobalPosition;
            GD.Print($"Current player position: {_currentPlayerPosition}");
            MoveBackGround();
            _previousPlayerPosition = _currentPlayerPosition;
            GD.Print($"Previous player position: {_previousPlayerPosition}");
            return _currentPlayerPosition;
        }
        public void MoveBackGround()
        {
            GD.Print($"Original offset: {ScrollOffset}");
            //Moving right
            if (_currentPlayerPosition.X > _previousPlayerPosition.X)
            {
                ScrollOffset -= _parallaxMagnitude;
                GD.Print($"Magnitude: {_parallaxMagnitude}");
                GD.Print($"New offset: {ScrollOffset}");
            }
            //Moving left
            else if (_currentPlayerPosition.X < _previousPlayerPosition.X)
            {
                ScrollOffset += _parallaxMagnitude;
            }

            if (ScrollOffset.X > RepeatSize.X * 0.5f * RepeatTimes)
            {
                ScrollOffset = new Vector2(ScrollOffset.X - (0.5f * RepeatTimes * RepeatSize.X), ScrollOffset.Y);
            }
            else if (ScrollOffset.X < RepeatSize.X * 0.5f * RepeatTimes)
            {
                ScrollOffset = new Vector2(ScrollOffset.X + (0.5f * RepeatTimes * RepeatSize.X), ScrollOffset.Y);
            } 
            GD.Print(ScrollOffset);
        }
    }
}

