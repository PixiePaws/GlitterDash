using Godot;
using System;


namespace UnicornGame
{
    public partial class FinishLine : Area2D
    {
        public override void _Ready()
        {
            BodyEntered += OnBodyEntered;
        }
        public void OnBodyEntered(Node node)
        {
            if (node is Player)
            {
                GD.Print("Level complete");
            }
        }
    }
}

