using Godot;
using System;

namespace UnicornGame
{
    public partial class GameManager : Node
    {
        public int score = 0;
        [Export] public Node2D RespawnPoint;

        public void RespawnPlayer()
        {
            Player p = GetNode<Player>("PlayerCharacter");
            p.GlobalPosition = RespawnPoint.GlobalPosition;
            p.RespawnPlayer();
        }

        public void AddScore()
        {
            score += 1;
            GD.Print("Score: " + score);
        }
    }
}
