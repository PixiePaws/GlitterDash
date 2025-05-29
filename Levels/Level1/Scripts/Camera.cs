using Godot;
using System;

namespace UnicornGame
{
	public partial class Camera : Camera2D
	{

		[Export]
		public Player Player { get; set; }
		private Vector2I size;

		public override void _Ready()
		{
			size = (Vector2I)GetViewportRect().Size;
			UpdateCameraPosition();
		}

		public override void _PhysicsProcess(double delta)
		{
			UpdateCameraPosition();
		}

		private void UpdateCameraPosition()
		{
			Vector2I currentCell = (Vector2I)(Player.GlobalPosition / size);
			GlobalPosition = (Vector2)currentCell * size;
		}
	}
}
