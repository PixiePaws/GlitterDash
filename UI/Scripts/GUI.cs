using Godot;
using System;


namespace UnicornGame
{
	public partial class GUI : Node
	{
		// Called when the node enters the scene tree for the first time.
		private Button _quitButton;
		private Control _settingsScene;
		public override void _Ready()
		{
			_quitButton = GetNode<Button>("QuitButton");
			_quitButton.Pressed += OnQuitButtonPressed;
		}

		public override void _Input(InputEvent @event)
		{
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
}