using Godot;
using System;

namespace UnicornGame
{
	public partial class GoldEgg : Area2D
	{
		[Export] private GameManager GameManager;
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			GameManager = GetNode<GameManager>("GameManager");
			BodyEntered += OnBodyEntered;
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public void OnBodyEntered(Node body)
		{
			GameManager.AddScore();
			QueueFree();
		}
	}
}
