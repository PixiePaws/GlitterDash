using Godot;
using System;

namespace UnicornGame
{
    public partial class Respawner : Node
    {
        private int _score = 0;
        public int Score
        {
            get { return _score; }
        }

        [Export] private Node2D _respawnPoint;
        [Export] private Label _scoreLabel;
        [Export] private Player _player;

        /// <summary>
        /// Respawn method that resets the player's position when they fall off the map.
        /// </summary>

        public void RespawnPlayer()
        {
            var ParentName = GetParent().Name;
            //GD.Print($"Parent Name : {ParentName}");
            string PlayerPath = $"/root/{ParentName}/PlayerCharacter";
            GD.Print(PlayerPath);
            _player = GetNode<Player>(PlayerPath);
            if (_player != null)
            {
                GD.Print("Got player reference");
            }
            else
            {
                GD.Print("Could not get player reference");
            }
            string RespawnPointPath = $"/root/{ParentName}/RespawnPoint";
            _respawnPoint = GetNode<Node2D>(RespawnPointPath);
            if (_respawnPoint != null)
            {
                GD.Print("Got respawnpoint reference");
            }
            else
            {
                GD.Print("Could not get respawnpoint reference");
            }
            ResetScore();
            _player.GlobalPosition = _respawnPoint.GlobalPosition;
        }

        /// <summary>
        /// Method to add score when the player collects a gold egg and updates the score label.
        /// </summary>
        public void AddScore()
        {
            var ParentName = GetParent().Name;
            GD.Print("Score added");
            string ScoreLabelPath = $"/root/{ParentName}/Camera2D/ScoreLabel";
            _scoreLabel = GetNode<Label>(ScoreLabelPath);
            _score += 1;
            _scoreLabel.Text = _score.ToString() + "/13";
        }

        public void ResetScore()
        {
            GD.Print("Score reset");
            _score = 0;
            _scoreLabel.Text = _score.ToString() + "/13";
        }
    }
}
