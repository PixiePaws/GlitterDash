using Godot;
using System;


namespace UnicornGame
{
	// This script is intended to handle screen mode settings, such as switching between fullscreen and windowed modes.

	// TODO: Make sure that the default screen mode is saved in the settings and update it to show the current screen mode

	public partial class ScreenMode : Node
	{
		private OptionButton _screenButton;
		private Label _screenLabel;
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			_screenButton = GetNode<OptionButton>("HBoxContainer/OptionButton");
			_screenLabel = GetNode<Label>("HBoxContainer/Label");

			_screenLabel.Text = "Screen Mode"; // Set the label text for screen mode
			_screenButton.ItemSelected += OnScreenModeSelected;
			// Initialize the screen mode button with options
			_screenButton.AddItem("Windowed");
			_screenButton.AddItem("Fullscreen");
			_screenButton.AddItem("Borderless");
		}

		// Called when an item is selected in the screen mode button
		private void OnScreenModeSelected(long index)

		{
			SetScreenMode((ScreenModeType)index);
		}



		private enum ScreenModeType
		{
			Windowed,
			Fullscreen,
			Borderless
		}
		private void SetScreenMode(ScreenModeType mode)
		{
			switch (mode)
			{
				case ScreenModeType.Windowed:
					// Set the window to windowed mode
					DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
					break;
				case ScreenModeType.Fullscreen:
					// Set the window to fullscreen mode
					DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
					break;
				case ScreenModeType.Borderless:
					// Set the window to borderless mode
					DisplayServer.WindowSetMode(DisplayServer.WindowMode.ExclusiveFullscreen);
					break;
			}
		}
	}
}