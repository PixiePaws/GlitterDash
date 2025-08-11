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
		public AudioStreamPlayer _birdProjectileFlyBy;
		public AudioStreamPlayer _spikeTrapHit;

		// player sounds
		public AudioStreamPlayer _jumpSound;
		public AudioStreamPlayer _walkSound;
		public AudioStreamPlayer _fallSound;
		public AudioStreamPlayer _wallSlideSound;


		public AudioStreamPlayer _portalSound;
		public AudioStreamPlayer _collectSound;

		private static AudioStreamPlayer _sfxPlayer;
		private static AudioStreamPlayer _sfx2Player;



		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			Instance = this;
			//GD.Print($"Audio manager path: {Instance.GetPath()}");
			_bgMusic = GetNode<AudioStreamPlayer>("Music/bgMusic");
			_circularSawHit = GetNode<AudioStreamPlayer>("SFX/CircularSawHit");
			_wreckingBallHit = GetNode<AudioStreamPlayer>("SFX/WreckingBallHit");
			_birdProjectileHit = GetNode<AudioStreamPlayer>("SFX/BirdProjectileHit");
			_birdProjectileFlyBy = GetNode<AudioStreamPlayer>("SFX/BirdProjectileFlyBy");
			_spikeTrapHit = GetNode<AudioStreamPlayer>("SFX/SpikeTrapHit");
			_jumpSound = GetNode<AudioStreamPlayer>("SFX/Jump");
			//_fallSound = GetNode<AudioStreamPlayer>("SFX/Falling");
			_wallSlideSound = GetNode<AudioStreamPlayer>("SFX/WallSlide");
			_walkSound = GetNode<AudioStreamPlayer>("SFX/Walk");
			_portalSound = GetNode<AudioStreamPlayer>("SFX/Portal");
			_collectSound = GetNode<AudioStreamPlayer>("SFX/CollectEgg");

			_sfxPlayer = new AudioStreamPlayer { Name = "SFXPlayer" };
			_sfx2Player = new AudioStreamPlayer { Name = "SFXPlayer" };

			AddChild(_sfxPlayer);
			AddChild(_sfx2Player);
		}

		public void PlayHitSound(Node node)
		{
			GD.Print($"Play hit sound was called in Audiomanager, triggered by {node.GetType()} {node.Name}");
			if (node is CircularSaw)
			{
				GD.Print("node is CircularSaw in PlayHitSound()");
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

		//Player sounds
		/*
		public void PlayJumpSound()
		{
			_jumpSound.Play();
		}
		public void PlayFallingSound()
		{
			_fallSound.Play();
		}
		public void PlayWallSlideSound()
		{
			_wallSlideSound.Play();
		}
		public void PlayWalkSound()
		{
			if (!_walkSound.Playing)
			{
				_walkSound.Play();
			}
		}
		public void StopWalkSound()
		{
			if (_walkSound.Playing)
			{
				_walkSound.Stop();
			}
		}

		public void PlayPortalSound()
		{
			_portalSound.Play();
		}

		public void PlayCollectSound()
		{
			_collectSound.Play();
		}*/

		// player movements
		public static void PlaySound(AudioStream sound)
		{
			if (_sfxPlayer != null && sound != null)
			{
				_sfxPlayer.Stream = sound;
				_sfxPlayer.Play();
			}
		}


		// others
		public static void PlaySound2(AudioStream sound)
		{
			if (_sfx2Player != null && sound != null)
			{
				_sfx2Player.Stream = sound;
				_sfx2Player.Play();
			}
		}

		public static void StopSound(AudioStream sound)
		{
			if (_sfxPlayer != null && sound != null)
			{
				_sfxPlayer.Stream = sound;
				_sfxPlayer.Stop();
			}
		}
	}
}
