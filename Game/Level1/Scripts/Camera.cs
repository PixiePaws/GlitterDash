             using Godot;
using System;

namespace UnicornGame
{
	public partial class Camera : Camera2D
	{

		[Export]
		public Player Player { get; set; }
		private Vector2I size;
		private Vector2 targetPosition;
		[Export] private float cameraSpeed = 1000f;

		public override void _Ready()
		{
			size = (Vector2I)GetViewportRect().Size;

			// Updates the camera position based on player's position
			UpdateCameraPosition();

			// Set the camera to begining position
			GlobalPosition = targetPosition;
		}

		public override void _PhysicsProcess(double delta)
		{
			// New position based on player's position
			UpdateCameraPosition();

			// Movement towards the target position
			GlobalPosition = GlobalPosition.MoveToward(targetPosition, (float)delta * cameraSpeed);

		}

		private void UpdateCameraPosition()
		{
			Vector2 playerPosition = Player.GlobalPosition;
			Vector2 cell = new Vector2(
				Mathf.Floor(playerPosition.X / size.X),
				Mathf.Floor(playerPosition.Y / size.Y)
			);
			// Calculates the target position
			targetPosition = cell * size;
		}
	}
}
