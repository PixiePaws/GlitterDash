using Godot;
using System;

namespace UnicornGame
{
	public partial class SettingsScript : Node
	{
		// TO DO - Make some kind of saving system for settings
		private Control _settingsScene;
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
			Node currentScene = GetTree().CurrentScene;
			if (currentScene.Name == "MainMenu")
			{
				if (Input.IsActionJustPressed("Settings"))
				{
					if (_settingsScene.Visible)
					{
						_settingsScene.Visible = false;
					}
					else
					{
						_settingsScene.Visible = true;
					}
				}
			}
			else
			{
				if (Input.IsActionJustPressed("Settings"))
				{
					if (_settingsScene.Visible)
					{
						_settingsScene.Visible = false;
					}
				}
			}
		}
	}
}