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
        [Export] private int _saveSlotsAmount = 3;
        private string _loadedSaveFile = "";
        private string _defaultFileName = "Save1.json";
        //private string _currentSaveFile;

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
        public string LoadedSaveFile
        {
            get { return _loadedSaveFile; }
            set { _loadedSaveFile = value; }
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
            bool IsLoadedSaveFileNull = string.IsNullOrEmpty(_loadedSaveFile);
            GD.Print($"IsLoadedSaveFileNull in JsonSaver: {IsLoadedSaveFileNull}");
            if (FileCount == 0 && IsLoadedSaveFileNull)
            {
                _fileName = _defaultFileName;
            }
            else if (FileCount < _saveSlotsAmount && IsLoadedSaveFileNull)
            {
                _fileName = $"{FileBodyName}{SaveNumberString}{FileExtensionString}";
            }
            else if (!IsLoadedSaveFileNull)
            {
                GD.Print($"_loadedSaveFile in JsonSaver is not null");
                _fileName = _loadedSaveFile;
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
                return;
            }
            else
            {
                Godot.Collections.Dictionary<string, Variant> LoadedDict = GetSaveFileAsDictionary(_savePath);
                Godot.Collections.Dictionary<string, Variant> NewLevelDict = (Godot.Collections.Dictionary<string, Variant>)Json.ParseString(GameData);
                //string GameDataKey = "";
                foreach (var Key in LoadedDict.Keys)
                {
                    //GameDataKey = Key as string;
                    if (NewLevelDict.ContainsKey(Key))
                    {
                        GD.Print($"GameData string contains key: {Key} in JsonSaver");
                        LoadedDict[Key] = NewLevelDict[Key];
                        GD.Print($"Replaced LoadedDict[{Key}] with GameData[{Key}]");
                        break;
                    }
                    else
                    {
                        LoadedDict[Key] = NewLevelDict[Key];
                        GD.Print($"Added key LoadedDict[{Key}] containing GameData[{Key}]");
                        break;
                    }
                }
                string UpdatedDictString = Json.Stringify(LoadedDict);
                try
                {
                    File.WriteAllText(_savePath, UpdatedDictString);
                    GD.Print($"Wrote UpdatedDictString into {_savePath}");
                }
                catch (Exception Error)
                {
                    GD.Print(Error);
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

