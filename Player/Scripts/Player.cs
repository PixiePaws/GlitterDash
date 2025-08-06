using Godot;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace UnicornGame
{
	public partial class Player : CharacterBody2D
	{
		[Export] public float Speed = 380f;
		[Export] public float JumpVelocity = -650f;
		[Export] public float WallJumpVelocity = -400f;
		[Export] public float WallJumpPush = 350f;
		[Export] public float WallSlideSpeed = 200f;
		[Export] public float Gravity = 1500f;
		[Export] public float JumpDuration = 1f;
		[Export] public float DashSpeed = 1000f;
		[Export] public float DashDuration = 0.25f;
		[Export] public float DashCooldown = 0.5f;
		[Export] public ColorRect Filter;
		[Export] public CollisionDetector _collisionDetector;
		[Export] public AnimatedSprite2D AnimatedSprite { get; set; } // tarvitaan animaatioita varten

		// Sounds 
		private AudioStream walkSound;
		private AudioStream jumpSound;
		private AudioStream fallSound;
		private AudioStream wallSlideSound;


		private bool _isWallSliding = false;
		private float _wallJumpDirection = 0;
		private int _jumpCount = 0;
		private float _jumpTimer = 0f;
		private bool _isDashing = false;
		private float _dashTimer = 0f;
		private float _dashCooldownTimer = 0f;
		private float _lastMoveDirection = 1f; // initial direction to the right
		private bool _justWallJumped = false;
		private float _justWallJumpedTimer = 0f;
		private bool _justJumped = false;
		private Godot.Vector2 _dashDirection = Godot.Vector2.Zero;
		// private CollisionDetector _collisionDetector;
		private bool _canControl = true;
		private bool walking = false;
		private AudioManager _audioManager;


		public AnimationPlayer _animatedSprite; // tarvitaan animaatioita varten
		public Respawner _respawner;


		public override void _Ready()
		{
			var ParentLevelPath = GetParent().GetPath();
			Filter = GetNode<ColorRect>($"{ParentLevelPath}/BlackWhite");
			_collisionDetector = GetNode<CollisionDetector>("CollisionDetector");
			if (_collisionDetector != null)
			{
				GD.Print("Successfully got collision detector reference");
			}
			_animatedSprite = GetNode<AnimationPlayer>("AnimationPlayer");
			_respawner = GetNode<Respawner>("../Respawner");

			//sounds
			_audioManager = GetNode<AudioManager>("/root/AudioManager");
			walkSound = GD.Load<AudioStream>("res://Audio/Sfx/walking-on-grass-quickly.mp3");
			jumpSound = GD.Load<AudioStream>("res://Audio/Sfx/jumpgrunt.wav");


			string currentScene = GetTree().CurrentScene.Name;
			// set the character facing right
			if (currentScene != "Level4")
			{
				var skeleton = GetNode<Node2D>("PartsSkeletonContainer");
				skeleton.Scale = new Vector2(-1, 1);
			}
			else
			{
				var skeleton = GetNode<Node2D>("PartsSkeletonContainer");
				skeleton.Scale = new Vector2(1, 1);
			}
		}


		public override void _PhysicsProcess(double delta)
		{
			if (Input.IsActionJustPressed("Die"))
			{
				Die();
			}
			if (!_canControl)
			{
				// If the player cannot control the character, skip the physics process
				return;
			}
			//_collisionDetector = GetNode<CollisionDetector>("res://Player/Scripts/CollisionDetector.cs");

			Godot.Vector2 velocity = Velocity;
			float inputDirection = Input.GetAxis("Move left", "Move right");

			// Handle dashing
			// Add timers
			if (_dashCooldownTimer > 0)
			{
				_dashCooldownTimer -= (float)delta;
			}

			if (_justWallJumpedTimer > 0)
			{
				_justWallJumpedTimer -= (float)delta;
				_justWallJumped = true; // Reset the wall jump state after the timer expires
			}
			else
			{
				_justWallJumped = false; // Reset the wall jump state after the timer expires
			}

			if (!_justWallJumped)
			{
				// Apply gravity
				if (!IsOnFloor() && !_isWallSliding)
				{
					velocity.Y += (float)(Gravity * delta);
				}
				BasicMovement(ref velocity, inputDirection);
				Jumping(ref velocity, inputDirection);
			}

			WallSlidingWallJumping(ref velocity, inputDirection, (float)delta);

			if (_isDashing)
			{
				_dashTimer -= (float)delta;

				if (_dashTimer <= 0)
				{
					_isDashing = false;
				}
				else
				{
					velocity = new Vector2(_dashDirection.X * DashSpeed, 0);
					_animatedSprite.Play("Dash");
					//_audioManager.PlayDashSound();
					Velocity = velocity;
					MoveAndSlide();
					return;
				}
			}

			HandleDash(inputDirection);

			if (_jumpTimer > 0)
			{
				_jumpTimer -= (float)delta;
			}

			if (!IsOnFloor() && !_isWallSliding && _jumpTimer <= 0 && Velocity.Y > 0 && !_justWallJumped)
			{
				if (_animatedSprite.CurrentAnimation != "Falling")
				{
					_animatedSprite.Play("Falling");
					//_audioManager.PlayFallingSound();
				}
			}

			// Updates the velocity based on the current state
			Velocity = velocity;
			if (_justWallJumpedTimer <= 0)
			{
				SetDirection(inputDirection); // Update the direction based on input
			}
			MoveAndSlide();

			//GD.Print(IsNearWall());
		}

		/// <summary>
		/// Handles basic movement logic for the player.
		/// Moves the player left or right based on input direction.
		/// </summary>
		private void BasicMovement(ref Godot.Vector2 velocity, float inputDirection)
		{
			if (inputDirection != 0)
			{
				velocity.X = inputDirection * Speed;
				_lastMoveDirection = inputDirection; // Update last move direction
				if (IsOnFloor() && !_isWallSliding && !_isDashing)
				{
					_animatedSprite.Play("Walk");
					if (!walking)
					{
						AudioManager.PlaySound(walkSound);
						walking = true;
					}
				}
			}
			else
			{
				velocity.X = Mathf.MoveToward(velocity.X, 0, Speed);

				if (IsOnFloor() && !_isWallSliding && !_isDashing)
				{
					_animatedSprite.Play("Idle");
					AudioManager.StopSound(walkSound);
					walking = false;
				}
			}
		}

		/// <summary>
		/// Handles jumping logic for the player.
		/// Handles both single and double jumps based on the player's state.
		/// </summary>
		/// <param name="velocity"></param>
		/// <param name="inputDirection"></param>
		private void Jumping(ref Godot.Vector2 velocity, float inputDirection)
		{
			// Handle jumping

			if (Input.IsActionJustPressed("Jump") && _jumpCount == 1)
			{
				velocity.Y = JumpVelocity;
				_jumpCount = 2;
				_animatedSprite.Stop();
				_animatedSprite.Play("Jump");
				//AudioManager.PlaySound(jumpSound);
				//_audioManager.PlayJumpSound();
				_justJumped = true;
				_jumpTimer = JumpDuration;
			}

			if (Input.IsActionJustPressed("Jump") && IsOnFloor())
			{
				velocity.Y = JumpVelocity;
				_jumpCount = 1;
				_animatedSprite.Play("Jump");
				//AudioManager.PlaySound(jumpSound);
				//_audioManager.PlayJumpSound();
				_justJumped = true;
				_jumpTimer = JumpDuration;
			}

			if (!Input.IsActionPressed("Jump") && IsOnFloor())
			{
				_jumpCount = 0;
				_justJumped = false;
			}

			if (!IsOnFloor() && !_isWallSliding && !_justJumped && _jumpTimer <= 0)
			{
				if (_animatedSprite.CurrentAnimation != "Falling")
				{
					_animatedSprite.Play("Falling");
					//_audioManager.PlayFallingSound();
				}
			}

			/*if (_justJumped && Mathf.Abs(Input.GetAxis("Move left", "Move right")) > 0.1f && IsOnFloor())
			{
				AudioManager.PlaySound(walkSound);
			}*/
		}

		/// <summary>
		/// Handles wall sliding and wall jumping logic for the player.
		/// Wall sliding occurs when the player is against a wall and not on the floor.
		/// </summary>
		private void WallSlidingWallJumping(ref Godot.Vector2 velocity, float inputDirection, float delta)
		{
			_isWallSliding = false;

			int wallDirection = GetWallDirection();

			/*
				int wallLayerId = 2;
				Vector2I tilePos = TileMap.LocalToMap(GlobalPosition);
				int wallTileId = TileMap.GetCellSourceId(wallLayerId, tilePos);

				// Wall slide
				if (wallTileId != -1)
				{ */
			if (IsNearWall() && !IsOnFloor() && inputDirection != 0 && MathF.Sign(inputDirection) == GetWallDirection())
			{
				_isWallSliding = true;
				velocity.Y = Mathf.Min(velocity.Y + Gravity * 0.5f, WallSlideSpeed);
				_jumpCount = 0;
				_animatedSprite.Play("WallSlide");
				//_audioManager.PlayWallSlideSound();

			}
			else if (IsNearWall() && !IsOnFloor() && inputDirection == 0)
			{
				_isWallSliding = false;
				_animatedSprite.Play("Falling");
				//_audioManager.PlayFallingSound();
			}

			// Wall jump
				if (_isWallSliding && Input.IsActionJustPressed("Jump"))
				{
					// If the player is wall sliding and presses jump while pressing towards the wall
					{
						GD.Print("WallDirection: " + wallDirection);
					if (wallDirection != 0)
					{
						velocity.X = WallJumpPush * -wallDirection;
						velocity.Y = WallJumpVelocity;
						// Vector2 direction = new Godot.Vector2(-wallDirection, -2).Normalized();
						// velocity = direction * WallJumpPush;
						_justWallJumped = true;
						_justWallJumpedTimer = 0.4f;
						_animatedSprite.Play("Jump");
						//_audioManager.PlayJumpSound();
					}

						SetDirection(-wallDirection);
						_isWallSliding = false; // Reset wall sliding state after jumping
					}
				}
		}

		/// <summary>
		/// Sets the direction of the player based on the input.
		/// This method also updates the wall checker raycast direction to match the player's facing direction.
		/// </summary>
		/// <param name="direction"></param>
		public void SetDirection(float direction)
		{
			if (direction == 1)
			{
				var skeleton = GetNode<Node2D>("PartsSkeletonContainer");
				skeleton.Scale = new Vector2(-1, 1);
				//_animatedSprite.FlipH = false; // Facing right
				GetNode<RayCast2D>("WallChecker").RotationDegrees = 180; // Wall checker fliped
			}
			else if (direction == -1)
			{
				var skeleton = GetNode<Node2D>("PartsSkeletonContainer");
				skeleton.Scale = new Vector2(1, 1);
				//_animatedSprite.FlipH = true; // Facing left
				GetNode<RayCast2D>("WallChecker").RotationDegrees = 0; // Wall checker fliped
			}
		}

		/// <summary>
		/// Checks if the player is near a wall using a RayCast2D node.
		/// </summary>
		/// <returns></returns>
		public bool IsNearWall()
		{
			var wallChecker = GetNode<RayCast2D>("WallChecker");
			return wallChecker.IsColliding();
		}
        public void Die()
        {
			walking = false;
            //CollisionLayer = 0; // <-- Remove player collision layer so the blood drops don't collide with it
			// !!!!!!!!!^^^^^^ Might have to change this manually back to CollisionLayer 1 when respawning ^^^^^^^^!!!!!!!!!!!!!!!

			// Load and instantiate the blood spray effect
			var bloodEffectScene = GD.Load<PackedScene>("res://Effects/Scenes/blood_particle_effect.tscn");
			var bloodEffect = bloodEffectScene.Instantiate<BloodParticleEffect>();

			// Place the effect at the GlobalPosition of the player
			bloodEffect.GlobalPosition = GlobalPosition;

			// Add it to the scene
			GetTree().CurrentScene.AddChild(bloodEffect);

			// Trigger spray logic
			bloodEffect.BloodSpray();

			// Remove the player (Doesn't work DON'T USE THIS)
			// QueueFree();

            // Hide the player <- This works
            Hide();

        }
		/// <summary>
		/// Gets the direction of the wall the player is currently sliding against.
		/// Returns 1 if the wall is on the right, -1 if on the left, and 0 if no wall is detected.
		/// </summary>
		private int GetWallDirection()
		{
			for (int i = 0; i < GetSlideCollisionCount(); i++)
			{
				var collision = GetSlideCollision(i);
				if (collision != null)
				{
					var normal = collision.GetNormal();
					if (Math.Abs(normal.X) > 0.9f)
					{
						return -(int)Mathf.Sign(normal.X); // 1 = seinä oikealla, -1 = vasemmalla
					}
				}
			}
			return 0;
		}

		/// <summary>
		/// Handles dashing. Dash moves to the direction that has last been applied.
		/// The dash is activated only if the cooldown is zero and the player is not
		/// already dashing.
		/// </summary>
		private void HandleDash(float inputDirection)
		{
			// Checks if the player presses the dash button and is not already dashing
			// and if the cooldown is over.
			if (Input.IsActionJustPressed("Dash") && !_isDashing && _dashCooldownTimer <= 0)
			{
				// Dash Direction
				_dashDirection = new Godot.Vector2(_lastMoveDirection, 0).Normalized();

				// Dashing on and initialize timers
				_isDashing = true;
				_dashTimer = IsOnFloor() ? DashDuration : DashDuration * 0.7f;
				_dashCooldownTimer = DashCooldown;
			}
		}

		/// <summary>
		/// Respawns the player at the respawn point and resets the player's state.
		/// This method is called when the player needs to respawn after dying.
		/// </summary>
		public void RespawnPlayer()
		{
			Show();
		}

		/// <summary>
		/// Handles the player's death and respawn logic based on the type of danger encountered.
		/// </summary>
		public async Task HandleDanger(string dangerType)
		{
			_collisionDetector.CallDeferred("set_process_mode", (int)ProcessModeEnum.Disabled);
			GD.Print("Player died");
			Visible = false;
			_canControl = false;

			if (dangerType == "dead")
			{
				await ToSignal(GetTree().CreateTimer(5f), Timer.SignalName.Timeout); // sama aika kun resetcameradie odotus
			}
			else if (dangerType == "fall")
			{
				await ToSignal(GetTree().CreateTimer(1f), Timer.SignalName.Timeout);
			}
			ResetPlayer();
		}

		/// <summary>
		/// Resets the player's position and state to the respawn point.
		/// </summary>
		public void ResetPlayer()
		{
			var skeleton = GetNode<Node2D>("PartsSkeletonContainer");
			skeleton.Scale = new Vector2(-1, 1);
			GlobalPosition = GetNode<Node2D>("../RespawnPoint").GlobalPosition;
			_collisionDetector.CallDeferred("set_process_mode", (int)ProcessModeEnum.Inherit);
			Visible = true;
			_canControl = true;
			Filter.Visible = false;
			_jumpCount = 0;

			// Reset the movements
			Velocity = Vector2.Zero;
			_isDashing = false;
			_dashTimer = 0f;
			_dashCooldownTimer = 0f;
			_justWallJumped = false;
			_justWallJumpedTimer = 0f;
			_isWallSliding = false;
			_justJumped = false;

			_animatedSprite.Play("Falling");
		}
	}
}