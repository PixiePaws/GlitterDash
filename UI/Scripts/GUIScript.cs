using Godot;
using System;

public partial class GUIScript : Node
{
	// Called when the node enters the scene tree for the first time.
	private Button _quitButton;
	public override void _Ready()
	{
		_quitButton = GetNode<Button>("QuitButton");
		_quitButton.Pressed += OnQuitButtonPressed;
	}

	private void OnQuitButtonPressed()
	{
		GetTree().Quit();
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	// public override void _Process(double delta)
	// {
	// }
}
