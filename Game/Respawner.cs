using Godot;
using System;

namespace UnicornGame
{
    public partial class Respawner : Node
    {
        public int score = 0;

        [Export] public Node2D RespawnPoint;
        [Export] public Label ScoreLabel;
        [Export] public CollisionDetector Detector;


        /// <summary>
        /// Respawn method that resets the player's position when they fall off the map.
        /// </summary>
        public void RespawnPlayer()
        {
            var parent = GetParent();
            Player p = GetNode<Player>("/root/Level1/PlayerCharacter");
            p.GlobalPosition = RespawnPoint.GlobalPosition;
            p.RespawnPlayer();
        }

        /// <summary>
        /// Method to add score when the player collects a gold egg and updates the score label.
        /// </summary>
        public void AddScore()
        {
            score += 1;
            ScoreLabel.Text = score.ToString() + "/13";
        }
    }
}
