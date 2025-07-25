using Godot;
using System;

namespace UnicornGame
{
	public partial class AudioManager : Node
	{
		public static AudioManager Instance;
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			Instance = this;
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
		}
	}
}
