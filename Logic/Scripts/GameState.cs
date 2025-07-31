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
            /*string File = GetSceneFilePath();
            if (File != null)
            {
                GD.Print("Got GameState filename");
            }
            var _parent = GetParent();
            GD.Print($"Parent name in GameState instance : {_parent.Name}");
            _player = GetNode<Player>($"/root/{GetParent().Name}/PlayerCharacter");
            if (_player != null)
            {
                GD.Print("got player reference in GameState");
            }
            else
            {
                GD.Print("player is null in GameState");
            }
            _respawner = GetNode<Respawner>($"/root/{GetParent().Name}/Respawner");
            if (_respawner != null)
            {
                GD.Print("got respawner reference in GameState");
            }*/
            string SystemTime = Time.GetDatetimeStringFromSystem();
            return new Godot.Collections.Dictionary<string, Variant>()
            {
                { "FilePath", GetParent().GetSceneFilePath()},
                { "Parent", GetParent().GetPath()},
                { "PlayerPositionX", GetNode<Player>($"/root/{GetParent().Name}/PlayerCharacter").GlobalPosition.X},
                { "PlayerPositionY", GetNode<Player>($"/root/{GetParent().Name}/PlayerCharacter").GlobalPosition.Y},
                { "EggsCollected", GetNode<Respawner>($"/root/{GetParent().Name}/Respawner").Score},
                { $"LevelCompleted", _currentScene.CurrentLevelCompleted},
                { "TimeStamp", $"{SystemTime}"}
            };
        }
    }
}