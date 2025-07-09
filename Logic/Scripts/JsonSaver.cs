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

        public string DirectoryPath
        {
            get { return _directoryPath;}
        }
        public string FileName
        {
            get { return _fileName;}
        }
        public string SavePath
        {
            get { return _savePath;}
        }
        public override void _Ready()
        {
            GD.Print("JsonSave _Ready()");
            //_saveData = new Godot.Collections.Dictionary();
            //_saveData.Add("name", "Matti");
            //_saveData.Add("Age", 50);

            //string jsonString = Json.Stringify(_saveData);
            _directoryPath = ProjectSettings.GlobalizePath("user://");
            _fileName = "Save1";

            //WriteTextToFile(_directoryPath, _fileName, jsonString);
        }
        public void WriteTextToFile(string DirectoryPath, string FileName, string GameData)
        {
            GD.Print("JsonSaver WriteTextToFile");
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

