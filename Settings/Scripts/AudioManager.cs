using Godot;
using System;

namespace UnicornGame
{
	public partial class AudioManager : Node
	{
		public static AudioManager Instance;
		public AudioStreamPlayer _bgMusic;
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			Instance = this;
			_bgMusic = GetNode<AudioStreamPlayer>("Music/bgMusic");

		}
	}
}
