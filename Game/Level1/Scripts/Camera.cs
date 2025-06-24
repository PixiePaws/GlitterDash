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

		private bool cameraToStart = false;
		private bool _pauseCamera = false;

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
			if (_pauseCamera)
				return;

			// New position based on player's position
			UpdateCameraPosition();

			if (cameraToStart)
			{
				GlobalPosition = targetPosition;
				cameraToStart = false;
			}
			else
			{
				// Movement towards the target position
				GlobalPosition = GlobalPosition.MoveToward(targetPosition, (float)delta * cameraSpeed);
			}
		}

		/// <summary>
		/// Method for the camera to follow the player
		/// </summary>
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

		/// <summary>
		/// Resets the camera position to start position and waits for the player respawn.
		/// The method pauses the camera that the player gets to see the die or
		/// the fall animation.
		/// </summary>
		/// <param name="type"></param>
		public async void ResetCamera(string type)
		{
			GD.Print("Resetting camera position to start");
			cameraToStart = true;

			_pauseCamera = true;

			if (type == "die")
			{
				await ToSignal(GetTree().CreateTimer(6f), "timeout");
			}
			else if (type == "fall")
			{
				await ToSignal(GetTree().CreateTimer(1f), "timeout");
			}
			_pauseCamera = false;

			UpdateCameraPosition();
		}
	}
}
