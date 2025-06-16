using Godot;
using System;

namespace UnicornGame
{
    public partial class Respawner : Node
    {
        public int score = 0;
        [Export] public Node2D RespawnPoint;
        [Export] public Label ScoreLabel;

        /// <summary>
        /// Respawn method that resets the player's position when they die or fall off the map.
        /// </summary>
        public void RespawnPlayer()
        {
            Player p = GetNode<Player>("PlayerCharacter");
            p.GlobalPosition = RespawnPoint.GlobalPosition;
            p.RespawnPlayer();
        }

        /// <summary>
        /// Method to add score when the player collects a gold egg and updates the score label.
        /// </summary>
        public void AddScore()
        {
            score += 1;
            ScoreLabel.Text = "Score: " + score.ToString();
        }
    }
}
