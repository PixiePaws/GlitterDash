using Godot;
using System;

namespace UnicornGame
{

	public partial class SettingsScreen : Node
	{

		// TO DO - Make sure that pressing esc key will close the settings screen and unpause the game

		[Export] private string _mainMenuPath = "res://MainMenu/Scenes/MainMenu.tscn"; // Path to the main menu scene
		private Button _resumeButton;
		private Button _mainMenuButton;
		private Button _quitButton;
		private Button _settingsButton;
		public override void _Ready()
		{
			GetTree().Paused = true; // Pause the game when settings screen is ready
			_resumeButton = GetNode<Button>("Control/VBoxContainer/ResumeButton");
			_resumeButton.Pressed += OnResumeButtonPressed;

			_mainMenuButton = GetNode<Button>("Control/VBoxContainer/MainMenuButton");
			_mainMenuButton.Pressed += OnMainMenuButtonPressed;

			_quitButton = GetNode<Button>("Control/VBoxContainer/QuitButton");
			_quitButton.Pressed += OnQuitButtonPressed;

			_settingsButton = GetNode<Button>("Control/VBoxContainer/SettingsButton");
			_settingsButton.Pressed += OnSettingsButtonPressed;
		}


		// public override void _Input(InputEvent @event)
		// {
		// 	if (Input.IsActionJustPressed("Settings"))
		// 	{
		// 		this.QueueFree(); // Remove the settings screen from the scene tree
		// 		GetTree().Paused = false; // Unpause the game
		// 	}
		// }

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		private void OnResumeButtonPressed()
		{
			GetTree().Paused = false; // Unpause the game
			this.QueueFree(); // Remove the settings screen from the scene tree
			GD.Print("Game resumed");
		}

		private void OnMainMenuButtonPressed()
		{
			GetTree().Paused = false; // Unpause the game before changing scenes
			// Load the main menu scene
			PackedScene mainMenuScene = ResourceLoader.Load<PackedScene>("res://MainMenu/Scenes/MainMenu.tscn");
			if (mainMenuScene != null)
			{
				GetTree().ChangeSceneToPacked(mainMenuScene);
			}
			else
			{
				GD.Print("Main menu scene not found");
			}
		}
		private void OnQuitButtonPressed()
		{
			GetTree().Quit(); // Quit the game
			GD.Print("Game quit");
		}
		private void OnSettingsButtonPressed()
		{
			Control settingsPanel = GetNodeOrNull<Control>("SettingsScene/Settings");
			if (settingsPanel != null)
			{
				if (settingsPanel.Visible == false)
				{
					settingsPanel.Visible = true; // Show settings scene
					GD.Print("Settings scene opened and game paused");
				}
			}
			else
			{
				GD.PrintErr("Settings scene not found!");
			}
		}
	}
}