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
        private Button _quitButton;
        public override void _Ready()
        {
            _quitButton = GetNode<Button>("LoadControl/TabBar/QuitButton");
            if (_quitButton != null)
            {
                GD.Print("Got Quit Button reference in LoadGame");
            }
            _quitButton.Pressed += OnQuitButtonPressed;
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
        }
        public void PopulateButtonList()
        {
            _buttonList = new Godot.Collections.Array<Button>();
            GD.Print(GetPath());
            var ControlNode = GetNode<Control>("LoadControl");
            if (ControlNode != null)
            {
                GD.Print("Couldn't get control node");
            }
            var MarginNode = ControlNode.GetNode<MarginContainer>("TabBar/MarginContainer");
            if (MarginNode != null)
            {
                GD.Print("Couldn't get margin node node");
            }
            var Vbox = MarginNode.GetNode<VBoxContainer>("LoadVBoxContainer");
            if (Vbox != null)
            {
                GD.Print("Couldn't get vbox node");
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
                GD.Print("Inside for loop");
                if (node is Button button)
                {
                    _buttonList.Add(button);
                    GD.Print("Added button to list");
                }
            }
            GD.Print("Button list populated");
        }
        public void PopulateSaveFileList()
        {
            int SaveFileAmount = DirAccess.GetFilesAt(_directoryPath).Length;
            _saveFileList = new string[SaveFileAmount];
            _saveFileList = DirAccess.GetFilesAt(_directoryPath);
            GD.Print("Savefile list populated");
        }
        public void ConnectButtonSignals()
        {
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
        public void OnButtonPressed(Button button)
        {
            /*for (int i = 0; i < _saveFileList.Length; i++)
            {
                GD.Print(_saveFileList[i]);
            }*/
            int SignalSenderIndex = button.GetIndex();
            GD.Print($"Button index: {SignalSenderIndex}");
            //Read the save file dynamically
            string FilePath = Path.Join(_directoryPath, _saveFileList[SignalSenderIndex]);
            GD.Print($"File path: {FilePath}");
            Godot.Collections.Dictionary<string, Variant> LoadedDictionary = GetSaveFileAsDictionary(FilePath);
            GD.Print(LoadedDictionary);
            //GD.Print("OnButtonPressed() was called");
            
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

        public void GetSaveDirectory()
        {
            PackedScene SaverScene = ResourceLoader.Load<PackedScene>(_saverScenePath);
        }
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
        public void SetButtonVisibilityAndText()
        {
            int SaveFileAmount = DirAccess.GetFilesAt(_directoryPath).Length;
            string[] TempArray = new string[SaveFileAmount];
            TempArray = DirAccess.GetFilesAt(_directoryPath);
            for (int i = 0; i < _buttonList.Count; i++)
            {
                if (i > SaveFileAmount - 1)
                {
                    //_buttonList[i].Disabled = true;
                    Color NewColor = _buttonList[i].Modulate;
                    NewColor.A = 0.7f;
                    _buttonList[i].Modulate = NewColor;
                    GD.Print("Visibility");
                }
                else
                {
                    _buttonList[i].Text = TempArray[i];
                }
            }
        }
        public void OnQuitButtonPressed()
        {
            QueueFree();
        }
    }
}

