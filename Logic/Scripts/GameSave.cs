using Godot;
using System;
using System.IO;

namespace UnicornGame
{
    public partial class JsonSaver : Node2D, IGameSaver
    {
        private Godot.Collections.Dictionary _saveData;
        [Export] private string _directoryPath;
        [Export] private string _savePath;
        [Export] private string _fileName;
        public override void _Ready()
        {
            _saveData = new Godot.Collections.Dictionary();
            _saveData.Add("name", "Teuvo");
            _saveData.Add("Age", 48);

            string jsonString = Json.Stringify(_saveData);
            _directoryPath = ProjectSettings.GlobalizePath("user://");
            _fileName = "Save1";

            WriteTextToFile(_directoryPath, _fileName, jsonString);
        }
        public void WriteTextToFile(string DirectoryPath, string FileName, string GameData)
        {
            if (!Directory.Exists(DirectoryPath))
            {
                Directory.CreateDirectory(DirectoryPath);
            }
            _savePath = Path.Join(_directoryPath, FileName);
            GD.Print(_savePath);
            try
            {
                File.WriteAllText(_savePath, GameData);
            }
            catch (Exception Error)
            {
                GD.Print(Error);
            }

        }

    }
}

