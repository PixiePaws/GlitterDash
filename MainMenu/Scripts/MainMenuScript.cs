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
		private Button _newGameButton;
		private Button _loadGameButton;
		private Button _settingsButton;
		private Button _continueButton;
		public override void _Ready()
		{
			int primaryScreen = DisplayServer.GetPrimaryScreen();

			Vector2I screenSize = DisplayServer.ScreenGetSize(primaryScreen);

			DisplayServer.WindowSetSize(screenSize);

			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);

			_quitButton = GetNode<Button>("Control/MarginContainer/VBoxContainer/QuitButton");
			_quitButton.Pressed += OnQuitButtonPressed;

			_newGameButton = GetNode<Button>("Control/MarginContainer/VBoxContainer/NewGameButton");
			_newGameButton.Pressed += OnNewGameButtonPressed;

			_loadGameButton = GetNode<Button>("Control/MarginContainer/VBoxContainer/LoadGameButton");
			_loadGameButton.Pressed += OnLoadGameButtonPressed;

			_settingsButton = GetNode<Button>("Control/MarginContainer/VBoxContainer/SettingsButton");
			_settingsButton.Pressed += OnSettingsButtonPressed;

			_continueButton = GetNode<Button>("Control/MarginContainer/VBoxContainer/ContinueButton");
			_continueButton.Pressed += OnContinueButtonPressed;
		}
		private void OnQuitButtonPressed()
		{
			GetTree().Quit();
		}
		private void OnContinueButtonPressed()
		{
			PackedScene SelectSaveScene = ResourceLoader.Load<PackedScene>(_selectSaveScenePath);
			LoadGame LoadGameScene = (LoadGame)SelectSaveScene.Instantiate();
			AddChild(LoadGameScene);
			string LastSavedSavePath = LoadGameScene.GetLastSavedSaveFilePath();
			GD.Print($"OnContinueButtonPressed(), LastSavedSavePath: {LastSavedSavePath}");
			Godot.Collections.Dictionary<string, Variant> SaveFileDict = LoadGameScene.GetSaveFileAsDictionary(LastSavedSavePath);
			LoadGameScene.ChangeSceneToNextLevel(SaveFileDict, LastSavedSavePath);
		}
		private void OnNewGameButtonPressed()
		{
			string SaveFileName;
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
		private void OnLoadGameButtonPressed()
		{
			PackedScene SelectSaveScene = ResourceLoader.Load<PackedScene>(_selectSaveScenePath);
			LoadGame LoadGameScene = (LoadGame)SelectSaveScene.Instantiate();
			AddChild(LoadGameScene);
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
		}
	}
}


