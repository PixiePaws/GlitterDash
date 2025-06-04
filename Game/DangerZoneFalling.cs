using Godot;
using System;

namespace UnicornGame
{
	public partial class DangerZoneFalling : Area2D
	{
		public override void _Ready()
		{
			// Connect the body entered signal to the OnBodyEntered method
			BodyEntered += OnBodyEntered;
		}


		public void OnBodyEntered(Node body)
		{
			if (body is Player player)
			{
				player.HandleDanger();
			}
		}
	}
}
