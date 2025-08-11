using Godot;
using System;

public partial class LevelsCompleted : Node2D
{
	private string levelSelectPath = "res://MainMenu/Scenes/LevelSelect.tscn";
	private string mainMenuPath = "res://MainMenu/Scenes/MainMenu.tscn";
	private Button _quitButton;
	private Button _levelSelectButton;
	private Button _mainMenuButton;
	public override void _Ready()
	{
		_quitButton = GetNode<Button>("Control/VBoxContainer/Quit");
		_levelSelectButton = GetNode<Button>("Control/VBoxContainer/LevelSelect");
		_mainMenuButton = GetNode<Button>("Control/VBoxContainer/MainMenu");

		_quitButton.Pressed += OnQuitButtonPressed;
		// _levelSelectButton += OnLevelSelectButtonPressed;
		// _mainMenuButton += OnMainMenuButtonPressed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	private void OnQuitButtonPressed()
	{
		GetTree().Quit();
	}

	private void OnMainMenuButtonPressed()
	{
		PackedScene selectMainMenuScene = ResourceLoader.Load<PackedScene>(mainMenuPath);
		if (selectMainMenuScene != null)
		{
			GetTree().ChangeSceneToPacked(selectMainMenuScene);
		}
		else
		{
			GD.Print("Level selection scene not found");
		}
	}
	private void OnLevelSelectButtonPressed()
	{
		PackedScene selectLevelSelectScene = ResourceLoader.Load<PackedScene>(levelSelectPath);
			if (selectLevelSelectScene != null)
			{
				GetTree().ChangeSceneToPacked(selectLevelSelectScene);
			}
			else
			{
				GD.Print("Level selection scene not found");
			}
	}
}
