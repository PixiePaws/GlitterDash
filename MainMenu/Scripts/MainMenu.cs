using Godot;
using System;

namespace UnicornGame
{
	// This class represents the main menu of the game.
	// It handles the visibility of the settings scene and pauses the game when settings are opened.

	public partial class MainMenu : Node
	{
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			GD.Print(GetTree().CurrentScene.Name);
		}
	}
}