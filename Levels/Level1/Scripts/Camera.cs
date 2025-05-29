using Godot;
using System;

namespace UnicornGame
{

	public partial class Camera : Camera2D
	{

		[Export]
		public Player _player { get; set; }
		private Vector2I _size;

		public override void _Ready()
		{
			_size = (Vector2I)GetViewportRect()._size;
			UpdateCameraPosition();
		}

		public override void _PhysicsProcess(double delta)
		{
			UpdateCameraPosition();
		}

		private void UpdateCameraPosition()
		{
			Vector2I currentCell = (Vector2I)(Player.GlobalPosition / _size);
			GlobalPosition = (Vector2)currentCell * _size;
		}
	}
}
