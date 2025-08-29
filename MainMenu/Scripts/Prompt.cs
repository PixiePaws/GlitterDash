using Godot;
using System;

namespace UnicornGame
{
    public partial class Prompt : CanvasLayer
    {
        [Export] private Button _okButton;
        [Export] private Button _cancelButton;

        public override void _Ready()
        {
            _okButton.Pressed += OnCancelButtonPressed;
            _cancelButton.Pressed += OnCancelButtonPressed;
        }

        public void OnOkButtonPressed()
        {
            GD.Print("Save file deleted!");
            Visible = false;
        }
        public void OnCancelButtonPressed()
        {
            Visible = false;
        }
    }
}

