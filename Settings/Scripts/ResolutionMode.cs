using Godot;
using System;
using System.Collections.Generic;

namespace UnicornGame
{
	public partial class ResolutionMode : Node
	{
		// TODO Make sure that this does something to the game resolution
		// Called when the node enters the scene tree for the first time.
		private OptionButton _resolutionButton;
		public override void _Ready()
		{
			_resolutionButton = GetNode<OptionButton>("HBoxContainer/OptionButton");
			_resolutionButton.ItemSelected += OnResolutionSelected;

			// Initialize the resolution button with options
			foreach (var resolution in _resolutions)
			{
				_resolutionButton.AddItem($"{resolution.Value.X} x {resolution.Value.Y}", resolution.Key);
				GD.Print("Added resolution option: " + $"{resolution.Value.X} x {resolution.Value.Y}");
			}
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		private Dictionary<int, Vector2I> _resolutions = new Dictionary<int, Vector2I>
	{
		{ 0, new Vector2I(1280, 720) }, // 720p
		{ 1, new Vector2I(1920, 1080) }, // 1080p
		{ 2, new Vector2I(1920, 1200) }, // 1080p
		{ 3, new Vector2I(2560, 1440) }, // 1440p
		{ 4, new Vector2I(3840, 2160) }  // 4K
	};
		private void OnResolutionSelected(long index)
		{
			SetResolution((int)index);
		}
		public void SetResolution(int index)
		{
			if (_resolutions.ContainsKey(index))
			{
				Vector2I resolution = _resolutions[index];
				DisplayServer.WindowSetSize(resolution);
				Vector2I screenSize = DisplayServer.ScreenGetSize(DisplayServer.WindowGetCurrentScreen());
				Vector2I calculatedCenterPosition = (screenSize - resolution) / 2;
				DisplayServer.WindowSetPosition(calculatedCenterPosition);
				GD.Print($"Resolution set to: {resolution}");
			}
			else
			{
				GD.Print("Invalid resolution index");
			}
		}
	}
}