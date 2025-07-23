using Godot;
using System;

namespace UnicornGame
{
    public partial class MovingSaw : Node2D
    {
        private bool ReachedTargetPosition = false;
        private Timer _timer;
        [Export] private StaticBody2D _circularSaw;
        [Export] private float _movementMagnitude = 1.0f;
        [Export] private Vector2 _targetPosition;
        private Vector2 _originalPosition;
        private Vector2 _currentPosition;
        public override void _Ready()
        {
            _circularSaw = GetNode<StaticBody2D>("CircularSaw");
            //GD.Print($"circular saw global position: {_circularSaw.GlobalPosition}");
            _originalPosition = _circularSaw.GlobalPosition;
            //GD.Print($"circular saw original position: {_originalPosition}");
            _currentPosition = _originalPosition;
            //GD.Print($"circular saw current position: {_currentPosition}");
            //_timer = new Timer();
        }
        public override void _Process(double delta)
        {
            MoveSaw(_originalPosition, _targetPosition, delta, _movementMagnitude);
        }

        public void MoveSaw(Vector2 OriginalPosition, Vector2 TargetPosition, double delta, float MovementMagnitude)
        {
            
            //GD.Print("Entered MoveSaw()");
            //GD.Print($"Movement magnitude: {MovementMagnitude}");
            
            //GD.Print($"current position: {_currentPosition}");
            if (_currentPosition.X < TargetPosition.X && !ReachedTargetPosition)
            {
                //GD.Print("Moving right");
                Vector2 TempVector = (Vector2.Right * MovementMagnitude) * (float)delta;
                //GD.Print($"Temp Vector: {TempVector}");
                _circularSaw.GlobalPosition += TempVector;
                _currentPosition = _circularSaw.GlobalPosition;
                if (_circularSaw.GlobalPosition.X >= TargetPosition.X)
                {
                    ReachedTargetPosition = true;
                    GD.Print(ReachedTargetPosition);
                }
            }
            if (_currentPosition.X > TargetPosition.X && !ReachedTargetPosition)
            {
                //GD.Print("Moving left");
                _circularSaw.GlobalPosition += (Vector2.Left * MovementMagnitude) * (float)delta;
                _currentPosition = _circularSaw.GlobalPosition;
                if (_currentPosition.X <= TargetPosition.X)
                {
                    ReachedTargetPosition = true;
                    GD.Print(ReachedTargetPosition);
                }
            }
            if (_currentPosition.X > OriginalPosition.X && ReachedTargetPosition)
            {
                //GD.Print("Moving back left");
                Vector2 TempVector = (Vector2.Left * MovementMagnitude) * (float)delta;
                //GD.Print($"Temp Vector: {TempVector}");
                _circularSaw.GlobalPosition += TempVector;
                _currentPosition = _circularSaw.GlobalPosition;
                if (_currentPosition.X <= OriginalPosition.X)
                {
                    ReachedTargetPosition = false;
                    //GD.Print("reached false");
                }
            }
            if (_currentPosition.X < OriginalPosition.X && ReachedTargetPosition)
            {
                //GD.Print("Moving back right");
                Vector2 TempVector = (Vector2.Right * MovementMagnitude) * (float)delta;
                //GD.Print($"Temp Vector: {TempVector}");
                _circularSaw.GlobalPosition += TempVector;
                _currentPosition = _circularSaw.GlobalPosition;
                if (_currentPosition.X >= OriginalPosition.X)
                {
                    ReachedTargetPosition = false;
                    //GD.Print("reached false");
                }
            }

            //GD.Print($"Moved saw, new position: {_circularSaw.GlobalPosition}");
            
        }

    }
}

