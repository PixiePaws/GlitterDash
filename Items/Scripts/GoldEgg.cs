using Godot;
using System;
using System.Collections.Generic;

namespace UnicornGame
{
	public partial class GoldEgg : Area2D
	{
		[Export] private Respawner _respawner;
		private AudioManager _audioManager;
		private AudioStream collectSound;



		public override void _Ready()
		{
			_audioManager = GetNode<AudioManager>("/root/AudioManager");
			collectSound = GD.Load<AudioStream>("res://Audio/Sfx/collectcoin.wav");
			
			_respawner = GetNode<Respawner>("../../Respawner");
			BodyEntered += OnBodyEntered;
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
			AudioManager.PlaySound2(collectSound);
            //_audioManager.PlayCollectSound();
			QueueFree();
		}

    }
}
