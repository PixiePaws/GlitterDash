using Godot;
using System;

namespace UnicornGame
{
	public partial class AudioManager : Node
	{
		public static AudioManager Instance;
		public AudioStreamPlayer _bgMusic;
		public AudioStreamPlayer _circularSawHit;
		public AudioStreamPlayer _wreckingBallHit;
		public AudioStreamPlayer _birdProjectileHit;
		public AudioStreamPlayer _spikeTrapHit;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			Instance = this;
			//GD.Print($"Audio manager path: {Instance.GetPath()}");
			_bgMusic = GetNode<AudioStreamPlayer>("Music/bgMusic");
			_circularSawHit = GetNode<AudioStreamPlayer>("SFX/CircularSawHit");
			_wreckingBallHit = GetNode<AudioStreamPlayer>("SFX/WreckingBallHit");
			_birdProjectileHit = GetNode<AudioStreamPlayer>("SFX/BirdProjectileHit");
			_spikeTrapHit = GetNode<AudioStreamPlayer>("SFX/SpikeTrapHit");

		}
		public void PlayHitSound(Node node)
		{
			GD.Print($"Play hit sound was called in Audiomanager, triggered by {node.Name}");
			if (node is CircularSaw)
			{
				_circularSawHit.Play();
			}
			if (node is Pendulum)
			{
				_wreckingBallHit.Play();
			}
			if (node is SpikeTrap)
			{
				_spikeTrapHit.Play();
			}
			if (node is BirdProjectile)
			{
				_birdProjectileHit.Play();
			}
		}
	}
}
