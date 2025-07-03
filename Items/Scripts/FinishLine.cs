using Godot;
using System;


namespace UnicornGame
{
    public partial class FinishLine : Area2D
    {
        public override void _Ready()
        {
            GD.Print("FinishLine _Ready()");
            BodyEntered += OnBodyEntered;
        }

        public void OnBodyEntered(Node node)
        {
            GD.Print($"FinishLine OnBodyEntered() was triggered by: {node.Name}");
            if (node is Player)
            {
                GD.Print("Level complete");
            }
        }
    }
}

