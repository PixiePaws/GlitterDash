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
            try
            {
                GD.Print("Respawning player");
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
                GameState CurrentState = GetNode<GameLevels>($"/root/{ParentName}/").CurrentGameState;
                if (CurrentState != null)
                {
                    GD.Print("Got CurrentGameState reference");
                }
                else
                {
                    GD.Print("Could not get CurrentGameState reference");
                }
                Godot.Collections.Dictionary<string, Variant> SaveData = CurrentState.SaveGameState();
                string JsonString = Json.Stringify(SaveData);
                IGameSaver GameSaver = GetNode<GameLevels>($"/root/{ParentName}/").GameManager.GameSaver;
                if (GameSaver != null)
                {
                    GD.Print("Got Game Saver reference succesfully");    
                }
                //GD.Print(GameSaver.DirectoryPath);
                //GD.Print(GameSaver.FileName);
                //GD.Print(JsonString);
                GameSaver.WriteTextToFile(GameSaver.DirectoryPath, GameSaver.FileName, JsonString);
                _player.GlobalPosition = _respawnPoint.GlobalPosition;
                }
            catch (Exception e)
            {
                GD.PrintErr($"Exception in RespawnPlayer: {e}");
            }
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

        /// <summary>
        /// Resets the score to zero
        /// </summary>
        public void ResetScore()
        {
            GD.Print("Score reset");
            _score = 0;
            _scoreLabel.Text = _score.ToString() + "/13";
        }
    }
}