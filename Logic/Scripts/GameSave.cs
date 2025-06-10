using Godot;
using System;
using System.IO;

namespace UnicornGame
{
    public partial class GameSave : Node2D
    {
        private Godot.Collections.Dictionary _saveData;
        private string _savePath;
        private string _fileName;
        public override void _Ready()
        {
            _saveData = new Godot.Collections.Dictionary();
            _saveData.Add("name", "Teuvo");
            _saveData.Add("Age", 48);

            string jsonString = Json.Stringify(_saveData);
            _savePath = ProjectSettings.GlobalizePath("user://");


        }
        private void WriteTextToFile(string SavePath, string FileName, string GameData)
        {
            if (!Directory.Exists(SavePath))
            {
                Directory.CreateDirectory(SavePath);
            }
        }

    }
}

