using Godot;
using System;
using System.Collections.Generic;

namespace UnicornGame
{
	public partial class GoldEgg : Area2D
	{
		[Export] private Respawner _respawner;
		[Export] public PackedScene GoldEggScene;

		private List<Vector2> _originalPositions = new();

		public override void _Ready()
		{
			_respawner = GetNode<Respawner>("../../Respawner");
			BodyEntered += OnBodyEntered;

			foreach (Node child in GetChildren())
			{
				if (child is GoldEgg goldEgg)
				{
					_originalPositions.Add(goldEgg.GlobalPosition);
				}
			}
		}

		/// <summary>
		/// Called when a body enters the area.
		/// This method calls for AddScore method in Respawner and then frees the GoldEgg node.
		/// It is connected to the BodyEntered signal of the Area2D node.
		/// </summary>
		/// <param name="body"></param>
		public void OnBodyEntered(Node body)
		{
			_respawner.AddScore();
			QueueFree();
		}

		public void ResetEggs()
		{
			_respawner.ResetScore();

			foreach (Vector2 pos in _originalPositions)
			{
				GoldEgg newEgg = GoldEgg.Instance<GoldEgg>();
				newEgg.GlobalPosition = pos;
				AddChild(newEgg);
			}
		}

        private static T Instance<T>()
        {
            throw new NotImplementedException();
        }
    }
}
