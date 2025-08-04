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
        private string _defaultFileName = "Save1.json";

        public string DirectoryPath
        {
            get { return _directoryPath; }
        }
        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }
        public string SavePath
        {
            get { return _savePath; }
        }
        public override void _Ready()
        {
            GD.Print("JsonSaver _Ready()");
            //_saveData = new Godot.Collections.Dictionary();
            //_saveData.Add("name", "Matti");
            //_saveData.Add("Age", 50);

            //string jsonString = Json.Stringify(_saveData);
            _directoryPath = ProjectSettings.GlobalizePath("user://");
            int FileCount = DirAccess.GetFilesAt(_directoryPath).Length;
            string FileBodyName = "Save";
            string SaveNumberString = (FileCount + 1).ToString();
            string FileExtensionString = ".json";
            if (FileCount == 0)
            {
                _fileName = _defaultFileName;
            }
            else
            {
                _fileName = $"{FileBodyName}{SaveNumberString}{FileExtensionString}";
            }
            //WriteTextToFile(_directoryPath, _fileName, jsonString);
        }
        public void WriteTextToFile(string DirectoryPath, string FileName, string GameData)
        {
            GD.Print("JsonSaver WriteTextToFile");
            GD.Print($"GameData string: {GameData}");
            if (!Directory.Exists(DirectoryPath))
            {
                Directory.CreateDirectory(DirectoryPath);
            }
            _savePath = Path.Join(_directoryPath, FileName);
            GD.Print(_savePath);
            if (!File.Exists(_savePath))
            {
                try
                {
                    File.WriteAllText(_savePath, GameData);
                }
                catch (Exception Error)
                {
                    GD.Print(Error);
                }
            }
            else
            {
                Godot.Collections.Dictionary<string, Variant> LoadedDict = GetSaveFileAsDictionary(_savePath);
                string GameDataKey = "";
                Godot.Collections.Dictionary<string, Variant> GameDataDict = (Godot.Collections.Dictionary<string, Variant>)GameData;
                foreach (var Key in LoadedDict.Keys)
                {
                    if (GameData.Contains(Key))

                        GameDataKey = Key;

                }
            }
        }
        public Godot.Collections.Dictionary<string, Variant> GetSaveFileAsDictionary(string FilePath)
        {
            if (!File.Exists(FilePath))
            {
                GD.Print("LoadGame OnButtonPressed() no save file at FilePath exists");
            }
            Godot.Collections.Dictionary<string, Variant> LoadedData = new Godot.Collections.Dictionary<string, Variant>();
            using var SaveFile = Godot.FileAccess.Open(FilePath, Godot.FileAccess.ModeFlags.Read);
            while (SaveFile.GetPosition() < SaveFile.GetLength())
            {
                var JsonString = SaveFile.GetLine();
                var JsonInstance = new Json();
                Error error = JsonInstance.Parse(JsonString);
                if (error != Error.Ok)
                {
                    GD.Print(error);
                    return LoadedData;
                }
                LoadedData = (Godot.Collections.Dictionary<string, Variant>)JsonInstance.Data;
            }
            return LoadedData;
        }
    }
}

