using Godot;
using System;

namespace UnicornGame
{
    public partial class Prompt : CanvasLayer
    {
        [Export] private Button _okButton;
        [Export] private Button _cancelButton;
        private TextureButton _onDeleteButtonPressedInstance;
        private LoadGame _loadGameScene;
        private string _saveFilePath;
        public override void _Ready()
        {
            _loadGameScene = (LoadGame)GetParent();
            _okButton.Pressed += OnOkButtonPressed;
            _cancelButton.Pressed += OnCancelButtonPressed;
        }

        public void OnOkButtonPressed()
        {
            _loadGameScene.DeleteSaveFile(_saveFilePath);
            GD.Print($"Save file at path: {_saveFilePath} deleted!");
            _onDeleteButtonPressedInstance.QueueFree();
            GD.Print("TextureButtonInstance deleted in OnOkButtonPressed() in Prompt");
            Visible = false;
        }
        public void OnCancelButtonPressed()
        {
            Visible = false;
        }
        public string SaveFilePath
        {
            get { return _saveFilePath; }
            set { _saveFilePath = value; }
        }
        public TextureButton OnDeleteButtonPressedInstance
        {
            get { return _onDeleteButtonPressedInstance; }
            set { _onDeleteButtonPressedInstance = value; }
        }
    }
}

