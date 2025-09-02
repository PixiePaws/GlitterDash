using Godot;
using System;

namespace UnicornGame
{
    public partial class GameManager : Node2D
    {
        private IGameSaver _gameSaver;
        [Export] private string _jsonSaverScenePath;
        private PackedScene _jsonSaverScene;
        private GameLevels _parentNode;
        private GameLevels.SaveFormat _saveFormat;
        
        public IGameSaver GameSaver
        {
            get { return _gameSaver; }
        }

        public override void _Ready()
        {
            _saveFormat = GetSaveFormat();
            switch (_saveFormat)
            {
                case GameLevels.SaveFormat.Json:
                    _jsonSaverScene = ResourceLoader.Load<PackedScene>("res://Logic/Scenes/JsonSaver.tscn");
                    _gameSaver = (IGameSaver)_jsonSaverScene.Instantiate();
                    AddChild((JsonSaver)_gameSaver);
                    break;
                    /*case GameLevels.SaveFormat.Xml:
                        _gameSaver = new XmlSaver();
                        break;*/
            }
            GD.Print($"GameManager parent node name: {_parentNode.Name}");
            BloodDecalManager.Instance.SetCurrentLevelScene(_parentNode);
        }
        public GameLevels.SaveFormat GetSaveFormat()
        {
            _parentNode = GetParent<GameLevels>();
            if (_parentNode != null)
            {
                return _parentNode.CurrentSaveFormat;
            }
            else
            {
                GD.Print("GameManager _parentNode is null!");
                return GameLevels.SaveFormat.None;
            }
        }
    }
}

