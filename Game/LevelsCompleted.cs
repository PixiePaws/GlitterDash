using Godot;
using System;

namespace UnicornGame
{
	public partial class LevelsCompleted : Node
	{
		// Change this to level selection when there is one
		private string _levelScenePath = "res://Game/Level1/Scenes/Level1.tscn"; // Path to the level scene
		private string _mainMenuPath = "res://MainMenu/Scenes/MainMenu.tscn";

		private Button _quitButton;
		private Button _levelSelectButton;
		private Button _mainMenuButton;
		public override void _Ready()
		{
			_levelSelectButton = GetNode<Button>("Control/VBoxContainer/LevelSelect");
			_levelSelectButton.Pressed += OnLevelSelectPressed;

			_mainMenuButton = GetNode<Button>("Control/VBoxContainer/MainMenu");
			_mainMenuButton.Pressed += OnMainMenuPressed;

			_quitButton = GetNode<Button>("Control/VBoxContainer/Quit");
			_quitButton.Pressed += OnQuitButtonPressed;
		}

		//Gets the level select scene
		private void OnLevelSelectPressed()
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

		// Gets the Main Menu scene
		private void OnMainMenuPressed()
		{
			PackedScene mainMenuScene = ResourceLoader.Load<PackedScene>(_mainMenuPath);
			if (mainMenuScene != null)
			{
				GetTree().ChangeSceneToPacked(mainMenuScene);
			}
			else
			{
				GD.Print("Main menu scene not found");
			}
		}

		// Quits the game
		private void OnQuitButtonPressed()
		{
			GetTree().Quit();
		}
	}
}



