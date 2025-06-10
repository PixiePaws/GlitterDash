using Godot;
using System;

namespace UnicornGame
{
	public partial class GoldEgg : Area2D
	{
		[Export] private GameManager GameManager;

		public override void _Ready()
		{
			GameManager = GetNode<GameManager>("/root/Level1/GameManager");
			BodyEntered += OnBodyEntered;
		}

		/// <summary>
		/// Called when a body enters the area.
		/// This method calls for AddScore method in GameManager and then frees the GoldEgg node.
		/// It is connected to the BodyEntered signal of the Area2D node.
		/// </summary>
		/// <param name="body"></param>
		public void OnBodyEntered(Node body)
		{
			GameManager.AddScore();
			QueueFree();
		}
	}
}
