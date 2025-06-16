using Godot;
using System;


namespace UnicornGame
{
	public partial class GUI : Node
	{
		// Called when the node enters the scene tree for the first time.
		[Export] private string _settingsScreenScenePath = "res://Settings/Scenes/SettingsScreen.tscn"; // Path to the settings screen scene
		private Button _quitButton;
		private Control _settingsScene;
		private PackedScene _selectSettingsScreenScene;
		private Button _selectSettings;

		public override void _Ready()
		{
			_selectSettingsScreenScene = ResourceLoader.Load<PackedScene>(_settingsScreenScenePath);

			_quitButton = GetNode<Button>("QuitButton");
			_quitButton.Pressed += OnQuitButtonPressed;

			// _selectSettings = GetNode<Button>("SettingsButton");
			// _selectSettings.Pressed += OnSettingsPressed;
		}


		public override void _Input(InputEvent @event)
		{
			if (Input.IsActionJustPressed("Settings" ))
			{
				OnSettingsPressed();
			}
		}

		private void OnQuitButtonPressed()
		{
			GetTree().Quit();
		}
		private void OnSettingsPressed() // This will bring settings to the user after pressing settings button
		{
			{
				if (_selectSettingsScreenScene != null && GetNodeOrNull<Node>("SettingsPanel") == null)
				{
					GD.Print("Settings button pressed in GUI");
					Node settingsPanel = _selectSettingsScreenScene.Instantiate();
					settingsPanel.Name = "SettingsPanel"; // IMPORTANT DONT DELETE
					AddChild(settingsPanel);
				}
				else
				{
					GD.Print("Settings scene not found");
				}
			}
		}
		// Called every frame. 'delta' is the elapsed time since the previous frame.
		// public override void _Process(double delta)
		// {
		// }

	}
}