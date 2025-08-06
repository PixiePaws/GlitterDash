using Godot;
using System;
using System.IO;

namespace UnicornGame
{
    public partial class GameState : Node2D
    {
        Player _player;
        GameLevels _parent;
        Respawner _respawner;
        GameLevels _currentScene;
        public override void _Ready()
        {
        }

        public Godot.Collections.Dictionary<string, Variant> SaveGameState()
        {
            _currentScene = (GameLevels)GetTree().CurrentScene;
            return new Godot.Collections.Dictionary<string, Variant>()
            {
                { $"{_currentScene.Name}", GetGameDataDictionary()}
            };
        }
        public Godot.Collections.Dictionary<string, Variant> GetGameDataDictionary()
        {
            string SystemTime = Time.GetDatetimeStringFromSystem(false, true);
            long UnixTime = Time.GetUnixTimeFromDatetimeString(SystemTime);
            return new Godot.Collections.Dictionary<string, Variant>()
            {
                { "FilePath", GetParent().GetSceneFilePath()},
                { "Parent", GetParent().GetPath()},
                { "PlayerPositionX", GetNode<Player>($"/root/{GetParent().Name}/PlayerCharacter").GlobalPosition.X},
                { "PlayerPositionY", GetNode<Player>($"/root/{GetParent().Name}/PlayerCharacter").GlobalPosition.Y},
                { "EggsCollected", GetNode<Respawner>($"/root/{GetParent().Name}/Respawner").Score},
                { $"LevelCompleted", _currentScene.CurrentLevelCompleted},
                { "TimeStamp", $"{SystemTime}"},
                { "UnixTime", $"{UnixTime}"}
            };
        }
    }
}