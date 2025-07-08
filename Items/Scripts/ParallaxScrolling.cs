using Godot;
using System;

namespace UnicornGame
{
    public partial class ParallaxScrolling : Parallax2D
    {
        [Export] private Player _playerCharacter;
        private Vector2 _previousPlayerPosition;
        private Vector2 _currentPlayerPosition;
        private float _playerPositionChangeX;
        private Vector2 _movementAmount;
        [Export] private Vector2 _parallaxMagnitude;
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
            //_parallaxMagnitude = new Vector2(0.2f, 0f);
            _previousPlayerPosition = Vector2.Zero;
        }
        public override void _Process(double delta)
        {
            GetPlayerPosition();
            MoveBackGround(_playerPositionChangeX);
        }


        public void GetPlayerPosition()
        {
            //GD.Print($"Player global position: {_playerCharacter.GlobalPosition}");
            _currentPlayerPosition = _playerCharacter.GlobalPosition;
            //GD.Print($"Current player position: {_currentPlayerPosition}");
            //GD.Print($"Previous player position: {_previousPlayerPosition}");
            _playerPositionChangeX = (_currentPlayerPosition.X - _previousPlayerPosition.X);
            //GD.Print($"Position change on X axis: {_playerPositionChangeX}");
            _previousPlayerPosition = _currentPlayerPosition;
            //GD.Print($"new Previous player position: {_previousPlayerPosition}");
        }
        public void MoveBackGround(float PlayerPositionChangeX)
        {
            //GD.Print($"Original offset: {ScrollOffset}");
            //GD.Print($"Original Player position change: {PlayerPositionChangeX}");
            //Moving right
            if (PlayerPositionChangeX > 0)
            {
                Vector2 tempRightVector = new Vector2(PlayerPositionChangeX, 0) * _parallaxMagnitude;
                GD.Print($"right vector: {tempRightVector}");
                ScrollOffset -= tempRightVector;
                //GD.Print($"Subtracted ScrollOffset: ");
                GD.Print($"Magnitude: {_parallaxMagnitude}");
                GD.Print($"New offset: {ScrollOffset}");
            }
            //Moving left
            else if (PlayerPositionChangeX < 0)
            {
                Vector2 tempLeftVector = new Vector2(Mathf.Abs(PlayerPositionChangeX), 0) * _parallaxMagnitude;
                GD.Print($"left vector: {tempLeftVector}");
                ScrollOffset += tempLeftVector;
            }

            if (ScrollOffset.X > RepeatSize.X)
            {
                ScrollOffset = new Vector2((ScrollOffset.X - RepeatSize.X), ScrollOffset.Y);
            }
            else if (ScrollOffset.X < 0)
            {
                ScrollOffset = new Vector2((ScrollOffset.X + RepeatSize.X), ScrollOffset.Y);
            }
            GD.Print(ScrollOffset);
        }
    }
}

