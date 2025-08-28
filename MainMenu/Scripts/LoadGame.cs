using Godot;
using System;
using System.IO;
using Godot.Collections;
using Godot.NativeInterop;

namespace UnicornGame
{
    public partial class LoadGame : CanvasLayer
    {
        Godot.Collections.Array<Button> _buttonList;
        private string[] _saveFileList;
        private string _saverScenePath;
        private string _directoryPath;
        private string _defaultLevelScenePath = "res://Game/Level1/Scenes/Level1.tscn";
        private Button _quitButton;
        private Button _okButton;
        private Button _cancelButton;
        private ConfirmationDialog _savesFullPrompt;
        private string _delButtonSpritePath = "res://Art/MainMenu/TrashCan3.png";
        public override void _Ready()
        {
            _savesFullPrompt = GetNode<ConfirmationDialog>("SavesFullPrompt");
            _okButton = _savesFullPrompt.GetOkButton();
            _cancelButton = _savesFullPrompt.GetCancelButton();
            _quitButton = GetNode<Button>("LoadControl/TabBar/QuitButton");
            if (_quitButton != null)
            {
                GD.Print("Got Quit Button reference in LoadGame");
            }
            string ParentName = GetParent().Name;
            //GD.Print($"Parent name in LoadGame _Ready() :{ParentName}");
            /*IGameSaver GameSaver = GetNode<GameLevels>($"/root/../").GameManager.GameSaver;
            if (GameSaver != null)
            {
                GD.Print("Got GameSaver reference in LoadGame");
            }
            else
            {
                GD.Print("Could't get GameSaver reference in LoadGame");
            }
            _directoryPath = GameSaver.DirectoryPath;*/
            _directoryPath = ProjectSettings.GlobalizePath("user://");
            PopulateButtonList();
            PopulateSaveFileList();
            CheckListLengthAndContents();
            ConnectButtonSignals();
            SetButtonVisibilityAndText();
            InstantiateDeleteButtons();
            GetAllLevelPaths();
        }
        public void PopulateButtonList()
        {
            _buttonList = new Godot.Collections.Array<Button>();
            GD.Print(GetPath());
            var ControlNode = GetNode<Control>("LoadControl");
            if (ControlNode != null)
            {
                GD.Print("Successfully got control node");
            }
            var MarginNode = ControlNode.GetNode<MarginContainer>("TabBar/MarginContainer");
            if (MarginNode != null)
            {
                GD.Print("Successfully got margin node");
            }
            var Vbox = MarginNode.GetNode<VBoxContainer>("LoadVBoxContainer");
            if (Vbox != null)
            {
                GD.Print("Successfully got vbox node");
            }
            /*var Vbox = GetNode<VBoxContainer>("Control/MarginContainer/VboxContainer");
            if (Vbox != null)
            {
                GD.Print("Successfully got Vbox in LoadGame");
            }
            else
            {
                GD.Print("Couldn't get Vbox in LoadGame ");
            }*/
            GD.Print(Vbox.Name);
            Godot.Collections.Array<Node> tempArray = Vbox.GetChildren();
            foreach (Node node in tempArray)
            {
                //GD.Print("Inside for loop");
                if (node is Button button)
                {
                    _buttonList.Add(button);
                    //GD.Print("Added button to list");
                }
            }
            GD.Print("Button list populated");
        }
        //Populates _saveFileList with string filenames of the files found in _directoryPath.
        public void PopulateSaveFileList()
        {
            int SaveFileAmount = DirAccess.GetFilesAt(_directoryPath).Length;
            _saveFileList = new string[SaveFileAmount];
            _saveFileList = DirAccess.GetFilesAt(_directoryPath);
            GD.Print("Savefile list populated");
        }
        //Connects the button signals to OnButtonPressed(ButtonCopy), which carries the information of the button instance, which was pressed.
        public void ConnectButtonSignals()
        {
            _quitButton.Pressed += OnQuitButtonPressed;
            _okButton.Pressed += OnOkButtonPressed;
            _cancelButton.Pressed += OnCancelButtonPressed;
            for (int i = 0; i < _saveFileList.Length; i++)
            {
                if (_buttonList[i] is Button button)
                {
                    Button ButtonCopy = button;
                    button.Pressed += () => OnButtonPressed(ButtonCopy);
                }
            }
            GD.Print("Signals connected");
        }
        //Loads the next uncompleted level according to the save file connected to the button.
        public void OnButtonPressed(Button button)
        {
            /*for (int i = 0; i < _saveFileList.Length; i++)
            {
                GD.Print(_saveFileList[i]);
            }*/
            int SignalSenderIndex = button.GetIndex();
            GD.Print($"Button index: {SignalSenderIndex}");
            //Read the save file dynamically
            string FileName = _saveFileList[SignalSenderIndex];
            string FilePath = Path.Join(_directoryPath, FileName);
            GD.Print($"File path: {FilePath}");
            Godot.Collections.Dictionary<string, Variant> LoadedDictionary = GetSaveFileAsDictionary(FilePath);
            GD.Print(LoadedDictionary);
            ChangeSceneToNextLevel(LoadedDictionary, FileName);
            //GD.Print("OnButtonPressed() was called");

        }
        //Returns the file path of a save file with the name FileName if found in _saveFileList.
        public string GetSaveFilePath(string FileName)
        {
            string FilePath = "";
            foreach (string File in _saveFileList)
            {
                if (File.Contains(FileName))
                    FilePath = Path.Join(_directoryPath, FileName);
            }
            return FilePath;
        }
        //Returns the contents of a save file at FilePath as a Godot.Collections.Dictionary.
        public Godot.Collections.Dictionary<string, Variant> GetSaveFileAsDictionary(string FilePath)
        {
            if (!File.Exists(FilePath))
            {
                GD.Print("LoadGame GetSaveFileAsDictionary() no save file at FilePath exists");
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

        public void GetSaveDirectory()
        {
            PackedScene SaverScene = ResourceLoader.Load<PackedScene>(_saverScenePath);
        }
        //Prints the amount of buttons in _buttonList and their names and the amount of save files in _saveFileList and their names.
        public void CheckListLengthAndContents()
        {
            GD.Print($"save file list length: {_saveFileList.Length}");
            GD.Print($"button list length: {_buttonList.Count}");
            for (int i = 0; i < _saveFileList.Length; i++)
            {
                GD.Print($"_saveFileList contains: {_saveFileList[i]}");
            }
            for (int i = 0; i < _buttonList.Count; i++)
            {
                GD.Print($"_buttonList contains: {_buttonList[i].Name}");
            }
        }
        /*
        Sets which buttons are visible and enabled and the text they contain. If a button is disabled and dont connected to a save file
        it is disabled with the text "Empty". Else, it shows the connected save file's name and the timestamp for when the file was last edited.
        */
        public void SetButtonVisibilityAndText()
        {
            int SaveFileAmount = DirAccess.GetFilesAt(_directoryPath).Length;
            string[] TempArray = new string[SaveFileAmount];
            //string[] TimeStampArray = new string[SaveFileAmount];
            TempArray = DirAccess.GetFilesAt(_directoryPath);
            for (int i = 0; i < _buttonList.Count; i++)
            {
                GD.Print($"SetButtonVisibilityAndText for loop, i: {i}");
                if (i > SaveFileAmount - 1)
                {
                    //_buttonList[i].Disabled = true;
                    Color NewColor = _buttonList[i].Modulate;
                    NewColor.A = 0.7f;
                    _buttonList[i].Modulate = NewColor;
                    //GD.Print("Visibility");
                }
                else
                {
                    string SaveFilePath = GetSaveFilePath(TempArray[i]);
                    //GD.Print($"SaveFilePath: {SaveFilePath}");
                    var GameDataDict = GetSaveFileAsDictionary(SaveFilePath);
                    //GD.Print($"GameDataDict: {GameDataDict}");
                    string TimeStamp = GetLatestTimeStampFromSave(GameDataDict);
                    //GD.Print($"SetButtonVisibilityAndText() TimeStamp: {TimeStamp}");
                    _buttonList[i].Text = $"{TempArray[i]} {TimeStamp}";
                    //GD.Print($"_buttonList[i].Text: {_buttonList[i].Text}, TempArray[i]: {TempArray[i]}, TimeStamp: {TimeStamp} ");
                }
            }
        }
        //Destroys this scene when quit button is pressed.
        public void OnQuitButtonPressed()
        {
            QueueFree();
        }
        //Returns an array of the level paths for the levels found in the file system.
        public Godot.Collections.Array GetAllLevelPaths()
        {
            Godot.Collections.Array LevelPathArray = new Godot.Collections.Array();
            String FileSystemPath = "res://Game/";
            String LevelSceneName = "";
            String LevelScenePath = "";
            string[] FileSystemDirectories = DirAccess.GetDirectoriesAt(FileSystemPath);
            foreach (String Directory in FileSystemDirectories)
            {
                GD.Print(Directory);
                if (Directory.Contains("Level"))
                {
                    LevelSceneName = $"{Directory}.tscn";
                    LevelScenePath = $"{FileSystemPath}/{Directory}/Scenes/{LevelSceneName}";
                    LevelPathArray.Add(LevelScenePath);
                }
            }
            foreach (string LevelPath in LevelPathArray)
            {
                GD.Print(LevelPath);
            }
            return LevelPathArray;
        }
        //Returns an array of the specified key DataKey's values on the second layer of the dictionary specified.
        public string[] GetKeysArrayFromSave(string DataKey, Godot.Collections.Dictionary<string, Variant> GameDataDict)
        {
            string[] KeysArray = new string[GameDataDict.Keys.Count];
            int index = 0;
            foreach (var Key in GameDataDict.Keys)
            {
                var LevelDataDict = (Godot.Collections.Dictionary<string, Variant>)GameDataDict[Key];
                if (LevelDataDict.ContainsKey(DataKey))
                {
                    KeysArray[index] = (string)LevelDataDict[DataKey];
                    index++;
                }
            }
            return KeysArray;
        }
        public string GetLatestTimeStampFromSave(Godot.Collections.Dictionary<string, Variant> GameDataDict)
        {
            string TimeStamp = "";
            long UnixTime = 0;
            foreach (var Key in GameDataDict.Keys)
            {
                GD.Print($"GetTimeStampFromSave() Key: {Key}");
                var LevelDataDict = (Godot.Collections.Dictionary<string, Variant>)GameDataDict[Key];
                if (LevelDataDict.ContainsKey("UnixTime"))
                {
                    if ((long)LevelDataDict["UnixTime"] > UnixTime)
                    {
                        UnixTime = (long)LevelDataDict["UnixTime"];
                        TimeStamp = (string)LevelDataDict["TimeStamp"];
                        GD.Print($"UnixTime: {UnixTime}, TimeStamp: {TimeStamp}");
                    }
                }
            }
            return TimeStamp;
        }
        //Checks the save file found in SaveFilePath for the last completed level and instantiates the next uncompleted level
        public void ChangeSceneToNextLevel(Godot.Collections.Dictionary<string, Variant> GameData, string SaveFilePath)
        {
            GD.Print("ChangeSceneToNextLevel was called");
            Godot.Collections.Dictionary<string, Variant> LevelData;
            string GameDataKey = "";
            foreach (var Key in GameData.Keys)
            {
                GD.Print("Iterating GameData keys");
                LevelData = (Godot.Collections.Dictionary<string, Variant>)GameData[Key];
                if (LevelData.ContainsKey("LevelCompleted") && (bool)LevelData["LevelCompleted"])
                {
                    //GD.Print($"Key: {Key}");
                    GameDataKey = Key;
                    //GD.Print(GameDataKey);
                    //GD.Print("LevelCompleted found and true");
                }
                else
                {
                    break;
                }
            }
            //GD.Print(GameDataKey);
            //GD.Print($"GameDataKey: {GameDataKey}");
            if (String.IsNullOrEmpty(GameDataKey))
            {
                GD.Print("GameDataKey is null");
                string DefaultGameDataKey = "Level1";
                GameDataKey = DefaultGameDataKey;
            }
            Godot.Collections.Array LevelPathArray = GetAllLevelPaths();
            string LevelScenePath = "";
            bool CurrentLevelFound = false;
            foreach (String LevelPath in LevelPathArray)
            {
                if (LevelPath.Contains(GameDataKey))
                {
                    GD.Print($"Level path {LevelPath} contains {GameDataKey}");
                    CurrentLevelFound = true;
                    continue;
                }
                LevelScenePath = LevelPath;
                if (CurrentLevelFound)
                {
                    break;
                }
            }
            GD.Print($"Next level path: {LevelScenePath}");
            PackedScene NextLevel = ResourceLoader.Load<PackedScene>(LevelScenePath);
            Node CurrentScene = GetTree().CurrentScene;
            GameLevels NextLevelInstance = (GameLevels)NextLevel.Instantiate();
            NextLevelInstance.LoadedSaveFile = SaveFilePath;
            CurrentScene.QueueFree();
            GetTree().Root.AddChild(NextLevelInstance);
            GetTree().CurrentScene = NextLevelInstance;
        }
        public string GetLastSavedSaveFilePath()
        {
            long UnixTime = 0;
            string SaveFilePath = "";
            Godot.Collections.Dictionary<string, Variant> GameDataDict;
            Godot.Collections.Dictionary<string, Variant> LevelDataDict;
            foreach (string SaveFile in _saveFileList)
            {
                string TempSaveFilePath = GetSaveFilePath(SaveFile);
                GD.Print($"TempSaveFilePath: {TempSaveFilePath}");
                GameDataDict = GetSaveFileAsDictionary(TempSaveFilePath);
                GD.Print($"GameDataDict in LoadGame GetLastSavedSaveFilePath(): {GameDataDict}");
                foreach (var Key in GameDataDict.Keys)
                {
                    GD.Print($"GetLastSavedSaveFilePath() TempSaveFilePath: {TempSaveFilePath}, Key: {Key}");
                    LevelDataDict = (Godot.Collections.Dictionary<string, Variant>)GameDataDict[Key];
                    //GD.Print($"LevelDataDict: {LevelDataDict}");
                    if (LevelDataDict.ContainsKey("UnixTime") && (long)LevelDataDict["UnixTime"] > UnixTime)
                    {
                        UnixTime = (long)LevelDataDict["UnixTime"];
                        SaveFilePath = TempSaveFilePath;
                        GD.Print($"GetLastSavedSaveFilePath() Key: {Key} UnixTime: {UnixTime}");
                    }
                    else
                    {
                        GD.Print($"GetLastSavedSaveFilePath() loop else Key: {Key} UnixTime: {UnixTime}");
                    }
                    GD.Print($"SaveFilePath: {SaveFilePath}");
                }
            }
            return SaveFilePath;
        }
        public void InstantiateDeleteButtons()
        {
            _delButtonSpritePath = "res://Art/MainMenu/TrashCan3.png";
            Texture2D DelButtonSprite = ResourceLoader.Load<Texture2D>(_delButtonSpritePath);
            var DeleteVboxContainer = GetNode<VBoxContainer>("LoadControl/TabBar/MarginContainer2/DeleteVBoxContainer");
            for (int i = 0; i < _buttonList.Count; i++)
            {
                TextureButton DelButton = new TextureButton();
                DelButton.TextureNormal = DelButtonSprite;
                DelButton.Name = $"DeleteButton{i + 1}";
                DelButton.Pressed += OnDeleteButtonPressed;
                DeleteVboxContainer.AddChild(DelButton);
            }
        }
        public void OnDeleteButtonPressed()
        {

        }
        public void OnOkButtonPressed()
        {
            _savesFullPrompt.Visible = false;
        }
        public void OnCancelButtonPressed()
        {
            QueueFree();
        }
        public bool SavesFullPromptVisibility
        {
            set { _savesFullPrompt.Visible = value; }
        }
    }
}

