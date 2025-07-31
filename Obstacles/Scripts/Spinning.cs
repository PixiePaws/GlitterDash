using Godot;
using System;
using System.IO;

namespace UnicornGame
{
    public partial class Spinning : Sprite2D
    {
        [Export] private float _rotationAmount = 1800.0f;

        public override void _Process(double delta)
        {
            RotateSaw(delta);
        }
        private void RotateSaw(double delta)
        {
            RotationDegrees += _rotationAmount * (float)delta;
        }
    }
}
