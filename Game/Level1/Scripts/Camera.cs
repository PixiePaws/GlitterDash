using Godot;
using System;


namespace UnicornGame
{
	public partial class Camera : Camera2D
	{
		[Export]
		public Player Player { get; set; }
		private TextureRect Clouds;
		private TextureRect Ground;
		private TextureRect UnderGround;
		private Vector2 originalCloudsPos;
		private Vector2 originalGroundPos;
		private Vector2 originalUndeGroundPos;



		private Vector2I size;
		private Vector2 targetPosition;
		[Export] private float cameraSpeed = 1000f;

		private bool cameraToStart = false;
		private bool _pauseCamera = false;

		private int previousCellY = 0;

		public override void _Ready()
		{
			Clouds = GetNode<TextureRect>("Clouds");
			Ground = GetNode<TextureRect>("Ground");
			UnderGround = GetNode<TextureRect>("UnderGround");

			originalCloudsPos = Clouds.GlobalPosition;
			originalGroundPos = Ground.GlobalPosition;
			originalUndeGroundPos = UnderGround.GlobalPosition;

			size = (Vector2I)GetViewportRect().Size;

			// Updates the camera position based on player's position
			UpdateCameraPosition();

			// Set the camera to begining position
			GlobalPosition = targetPosition;

			previousCellY = (int)Mathf.Floor(Player.GlobalPosition.Y / size.Y);
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
			int cellY = (int)Mathf.Floor(playerPosition.Y / size.Y);


			targetPosition = new Vector2(
				Mathf.Floor(playerPosition.X / size.X) * size.X,
				cellY * size.Y
			);

			UpdateBackground(cellY);
			previousCellY = cellY;
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

			Vector2 offset = targetPosition - new Vector2(0, 0); 
			
			Clouds.GlobalPosition = originalCloudsPos + offset;
			Ground.GlobalPosition = originalGroundPos + offset;
			UnderGround.GlobalPosition = originalUndeGroundPos + offset;

		}
		private void UpdateBackground(int currentCellY)
		{
			int shiftAmount = size.Y;

			if (currentCellY != previousCellY)
			{
				int direction = currentCellY > previousCellY ? -1 : 1;

				Vector2 shift = new Vector2(0, direction * shiftAmount);

				Clouds.GlobalPosition += shift;
				Ground.GlobalPosition += shift;
				UnderGround.GlobalPosition += shift;

			}

		}

	}
}
