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
        }
        public override void _PhysicsProcess(double delta)
        {
            GetPlayerPosition();
        }

        public void GetPlayerPosition()
        {
            
            _previousPlayerPosition = _currentPlayerPosition;
        }
    }
}

