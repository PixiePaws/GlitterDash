using Godot;
using System;

namespace UnicornGame
{
    public partial class CircularSaw : Sprite2D
    {
        [Export] private float RotationAmount = 2.0f;
        [Export] private int RotateSawCallAmount = 2000;
        public override void _Process(double delta)
        {
            for (int i = 0; i < RotateSawCallAmount; i++)
            {
                RotateSaw(delta);
            }
        }
        private void RotateSaw(double delta)
        {
            RotationDegrees += RotationAmount * (float)delta;
        }
    }
}
