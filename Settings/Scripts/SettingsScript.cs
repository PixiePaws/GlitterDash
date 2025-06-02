using Godot;
using System;

namespace UnicornGame
{
	public partial class SettingsScript : Node
	{
		public Control _settingsScene;
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			GD.Print("SettingsScript _Ready called");
			_settingsScene = GetNode<Control>("Settings");
			if (_settingsScene != null)
			{
				_settingsScene.Visible = false; // Ensure settings scene is initially hidden
				GD.Print("Settings scene initialized and hidden");
			}
			else
			{
				GD.PrintErr("Settings scene not found!");
			}

		}
		public override void _Input(InputEvent @event)
		{
			OnSettingsPressed();

		}
		private void OnSettingsPressed()
		{
			if (_settingsScene.Visible == false && Input.IsActionJustPressed("Settings"))
			{

				GD.Print("Settings button pressed in GUI");

				_settingsScene.Visible = true; // Show settings scene

				GetTree().Paused = true; // Pause the game
				GD.Print("Game paused");

			}
			else
			{
				if (_settingsScene.Visible == true && Input.IsActionJustPressed("Settings"))
				{
					GD.Print("Settings button pressed in GUI to hide settings");

					_settingsScene.Visible = false; // Hide settings scene

					GetTree().Paused = false; // Unpause the game
					GD.Print("Game unpaused");
				}
			}
		}
	}
}