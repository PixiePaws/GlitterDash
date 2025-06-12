using Godot;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace UnicornGame
{

    public partial class Player : CharacterBody2D
    {
        [Export] public float Speed = 200f;
        [Export] public float JumpVelocity = -600f;
        [Export] public float WallJumpVelocity = -600f;
        [Export] public float WallJumpPush = 1500f;
        [Export] public float WallSlideSpeed = 100f;
        [Export] public float Gravity = 1500f;
        [Export] public float DashSpeed = 800f;
        [Export] public float DashDuration = 0.2f;
        [Export] public float DashCooldown = 0.5f;
        //[Export] public TileMap TileMap;

        private bool _isWallSliding = false;
        private float _wallJumpDirection = 0;
        private int _jumpCount = 0;
        private bool _isDashing = false;
        private float _dashTimer = 0f;
        private float _dashCooldownTimer = 0f;
        private float _lastMoveDirection = 1f; // initial direction to the right
        private bool _justWallJumped = false;
        private float _justWallJumpedTimer = 0f;
        private Godot.Vector2 _dashDirection = Godot.Vector2.Zero;
        // private CollisionDetector _collisionDetector;
        private bool _canControl = true;
        public AnimatedSprite2D _animatedSprite; // tarvitaan animaatioita varten


        public override void _Ready()
        {
            _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        }

        public override void _PhysicsProcess(double delta)
        {
            if (!_canControl)
            {
                // If the player cannot control the character, skip the physics process
                return;
            }
            //_collisionDetector = GetNode<CollisionDetector>("res://Player/Scripts/CollisionDetector.cs");

            //if (_collisionDetector.health > 0)
            //{
            Godot.Vector2 velocity = Velocity;
            float inputDirection = Input.GetAxis("Move left", "Move right");

            // Handle dashing
            // Add timers
            if (_dashCooldownTimer > 0)
            {
                _dashCooldownTimer -= (float)delta;
            }

            HandleDash(inputDirection);

            if (_isDashing)
            {
                _dashTimer -= (float)delta;

                if (_dashTimer <= 0)
                {
                    _isDashing = false;
                }
                else
                {
                    velocity = _dashDirection * DashSpeed;
                    _animatedSprite.Play("Dash");
                    Velocity = velocity;
                    MoveAndSlide();
                    return;
                }
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

            // Updates the velocity based on the current state
            Velocity = velocity;
            SetDirection(inputDirection); // Update the direction based on input
            MoveAndSlide();

            GD.Print(IsNearWall());
            //}
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
                _animatedSprite.Play("Walk");
            }
            else
            {
                velocity.X = Mathf.MoveToward(velocity.X, 0, Speed);

                if (IsOnFloor() && !_isWallSliding && !_isDashing)
                {
                    _animatedSprite.Play("Basic");
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
                _animatedSprite.Play("Jump");
            }

            if (Input.IsActionJustPressed("Jump") && IsOnFloor())
            {
                velocity.Y = JumpVelocity;
                _jumpCount = 1;
                _animatedSprite.Play("Jump");
            }

            if (!Input.IsActionPressed("Jump") && IsOnFloor())
            {
                _jumpCount = 0;
            }
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
            if (IsNearWall() && !IsOnFloor())
            {
                _isWallSliding = true;
                velocity.Y = Mathf.Min(velocity.Y + Gravity * 0.5f, WallSlideSpeed);
                _jumpCount = 0;
                _animatedSprite.Play("OnWall");
            }

            // Wall jump
            if (_isWallSliding && Input.IsActionJustPressed("Jump"))
            {
                // If the player is wall sliding and presses jump while pressing towards the wall
                {
                    GD.Print("WallDirection: " + wallDirection);
                    if (wallDirection != 0)
                    {
                        // Velocity.X = WallJumpPush * -wallDirection;
                        // Velocity.Y = WallJumpVelocity;
                        Vector2 direction = new Godot.Vector2(-wallDirection, -1).Normalized();
                        velocity = direction * WallJumpPush;
                        _justWallJumped = true;
                        _justWallJumpedTimer = 0.15f;
                        _animatedSprite.Play("Jump");
                    }

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
                _animatedSprite.FlipH = false; // Facing right
                GetNode<RayCast2D>("WallChecker").RotationDegrees = 0; // Wall checker fliped
            }
            else if (direction == -1)
            {
                _animatedSprite.FlipH = true; // Facing left
                GetNode<RayCast2D>("WallChecker").RotationDegrees = 180; // Wall checker fliped
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
                    var normal = collision.GetNormal(); // Oikea tapa hakea normaali Godot 4:ssa
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
                _dashTimer = DashDuration;
                _dashCooldownTimer = DashCooldown;
            }
        }

        /// <summary>
        /// Handles the player's death logic.
        /// </summary>
        public void Die()
        {
            GD.Print("Player died");
            // kuolemis animaation käynnistäminen
            // kuolemis äänen soittaminen

            _animatedSprite.Play("Die");
            Visible = false;
            _canControl = false;

            ResetPlayer();
        }

        /// <summary>
        /// Respawns the player at the respawn point and resets the player's state.
        /// This method is called when the player needs to respawn after dying.
        /// </summary>
        public void RespawnPlayer()
        {
            Show();
            // kameran resetointi
            //_collisionDetector.health = 1; // Reset health
        }

        /// <summary>
        /// Handles the player's death and respawn logic.
        /// </summary>
        public async Task HandleDanger()
        {
            GD.Print("Player died");
            Visible = false;
            _canControl = false;
            await ToSignal(GetTree().CreateTimer(1.0f), Timer.SignalName.Timeout);

            ResetPlayer();
        }

        /// <summary>
        /// Resets the player's position and state to the respawn point.
        /// </summary>
        public void ResetPlayer()
        {
            GlobalPosition = GetNode<Node2D>("../RespawnPoint").GlobalPosition; // Assuming RespawnPoint is a Node2D
            Visible = true;
            _canControl = true;
            _jumpCount = 0;
            _animatedSprite.Play("Falling");
        }
    }
}