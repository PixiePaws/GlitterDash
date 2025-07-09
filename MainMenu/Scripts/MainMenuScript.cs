using Godot;
using System;

namespace UnicornGame
{
	public partial class MainMenuScript : Node
	{
		// Called when the node enters the scene tree for the first time.
		// This script handles the main menu functionality, including starting the game, opening settings, and quitting the game.
		// It connects button signals to their respective methods for handling user interactions.

		// Change this to level selection when there is one
		private string _levelScenePath = "res://Game/Level1/Scenes/Level1.tscn"; // Path to the level scene
		private string _settingsScenePath = "res://Settings/Scenes/Settings.tscn"; // Path to the settings scene
		private string _selectSaveScenePath = "res://MainMenu/Scenes/LoadGame.tscn";
		private Button _quitButton;
		private Button _startButton;
		private Button _loadGameButton;
		// private Button _settingsButton;
		private Button _settingsButton;
		public override void _Ready()
		{
			int primaryScreen = DisplayServer.GetPrimaryScreen();

			Vector2I screenSize = DisplayServer.ScreenGetSize(primaryScreen);

			DisplayServer.WindowSetSize(screenSize);

			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);

			_quitButton = GetNode<Button>("MarginContainer/VBoxContainer/QuitButton");
			_quitButton.Pressed += OnQuitButtonPressed;

			_startButton = GetNode<Button>("MarginContainer/VBoxContainer/StartButton");
			_startButton.Pressed += OnStartButtonPressed;

			_loadGameButton = GetNode<Button>("MarginContainer/VBoxContainer/LoadGameButton");
			_loadGameButton.Pressed += OnLoadGameButtonPressed;

			_settingsButton = GetNode<Button>("MarginContainer/VBoxContainer/SettingsButton");
			_settingsButton.Pressed += OnSettingsButtonPressed;
		}
		private void OnQuitButtonPressed()
		{
			GetTree().Quit();
		}
		private void OnStartButtonPressed()
		{
			PackedScene selectLevelScene = ResourceLoader.Load<PackedScene>(_levelScenePath);
			if (selectLevelScene != null)
			{
				GetTree().ChangeSceneToPacked(selectLevelScene);
			}
			else
			{
				GD.Print("Level selection scene not found");
			}
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
				else
				{
					settingsPanel.Visible = false;
					GD.Print("Settings scene closed, and game unpaused.");
				}
			}
		}

		private void OnLoadGameButtonPressed()
		{
			PackedScene SelectSaveScene = ResourceLoader.Load<PackedScene>(_selectSaveScenePath);
			LoadGame LoadGameScene = (LoadGame)SelectSaveScene.Instantiate();
			AddChild(LoadGameScene);
		}
		// private void OnSettingsButtonPressed()
		// {
		// 	CanvasLayer settingsScene = GetNode<CanvasLayer>("SettingsScene");
		// 	if (settingsScene != null)
		// 	{
		// 		if (settingsScene.Visible == false)
		// 		{
		// 			settingsScene.Visible = true; // Show settings scene
		// 			GetTree().Paused = true; // Pause the game
		// 			GD.Print("Settings scene opened and game paused");
		// 		}
		// 	}
		// }
	}
}


