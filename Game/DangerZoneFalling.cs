using Godot;
using System;

namespace UnicornGame
{
	public partial class DangerZoneFalling : Area2D
	{
		[Export] public GoldEggManager _goldeggmanager;
		[Export] public Camera Camera;
		[Export] private Respawner _respawner;
		private bool _gameStarted = false;

		public override void _Ready()
		{
			// Connect the body entered signal to the OnBodyEntered method
			BodyEntered += OnBodyEntered;

			// Timer to delay the start of the game so that it doesn't call onbodyentered immediately
			GetTree().CreateTimer(0.5f).Timeout += () => _gameStarted = true;
		}

		/// <summary>
		/// When player falls of the map the method calls for respawning the player
		/// and reseting the eggs
		/// </summary>
		/// <param name="body"></param>
		public void OnBodyEntered(Node body)
		{
			if (!_gameStarted) return;

			Camera?.ResetCamera("fall");

			if (body is Player player)
			{
				player?.HandleDanger("fall");
			}
			_respawner.RespawnPlayer();
			_goldeggmanager.ResetEggs();
		}
	}
}
