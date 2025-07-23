using Godot;
using System;
using System.IO;

namespace UnicornGame
{
    public partial class CircularSaw : Sprite2D
    {
        [Export] private float _rotationAmount = 180.0f;
        [Export] private int _rotateSawCallAmount = 2000;
        public override void _Process(double delta)
        {
            RotateSaw(delta);
        }
        private void RotateSaw(double delta)
        {
            RotationDegrees += _rotationAmount * (float)delta * _rotateSawCallAmount;
        }
    }
}
