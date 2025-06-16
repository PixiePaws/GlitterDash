using Godot;
using System;

namespace UnicornGame
{
    public partial class GameManager : Node
    {
        private IGameSaver _gameSaver;
        [Export] private SaveFormat _saveFormat;

        public enum SaveFormat
        {
            Xml,
            Json
        }
        public override void _Ready()
        {
            switch (_saveFormat)
            {
                case SaveFormat.Json:
                    _gameSaver = new JsonSaver();
                    break;
                /*case SaveFormat.Xml:
                    _gameSaver = new XmlSaver();
                    break;
                */
            }
        }
    }
}

