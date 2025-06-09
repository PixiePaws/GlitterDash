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
        private Godot.Vector2 _dashDirection = Godot.Vector2.Zero;
        // private CollisionDetector _collisionDetector;
        private bool _canControl = true;

        [Export]
        public AnimatedSprite2D AnimatedSprite { get; set; } // tarvitaan animaatioita varten


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
                    Velocity = velocity;
                    MoveAndSlide();
                    return;
                }
            }
            // Apply gravity
            if (!IsOnFloor() && !_isWallSliding)
            {
                velocity.Y += (float)(Gravity * delta);
            }

            HandleDash(inputDirection);


            if (!_justWallJumped)
            {
                BasicMovement(ref velocity, inputDirection);
            }
            else
            {
                _justWallJumped = false;
            }

            Jumping(ref velocity, inputDirection);
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
            }
            else
            {
                velocity.X = Mathf.MoveToward(velocity.X, 0, Speed);
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
            }

            if (Input.IsActionJustPressed("Jump") && IsOnFloor())
            {
                velocity.Y = JumpVelocity;
                _jumpCount = 1;
            }

            if (!Input.IsActionPressed("Jump") && IsOnFloor())
            {
                _jumpCount = 0;
            }
        }

        /// <summary>
        /// Handles wall sliding and wall jumping logic for the player.
        /// Wall sliding occurs when the player is against a wall and not on the floor.
        /// Wall jumping allows the player to jump off.
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
                        Vector2 WallJumpDirectionVector = new Godot.Vector2(-wallDirection * 3f, -1).Normalized();
                        velocity = WallJumpDirectionVector * WallJumpPush;
                        _justWallJumped = true;
                    }

                    _isWallSliding = false; // Reset wall sliding state after jumping
                }
            }
        }

        public void SetDirection(float direction)
        {
            if (direction == 1)
            {
                //AnimatedSprite.FlipH = false; // Facing right
                GetNode<RayCast2D>("WallChecker").RotationDegrees = 0;
            }
            else if (direction == -1)
            {
                //AnimatedSprite.FlipH = true; // Facing left
                GetNode<RayCast2D>("WallChecker").RotationDegrees = 180; // Wall checker should also flip
            }
        }

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
                // Dash direction based on input
                float direction;
                if (_lastMoveDirection != 0)
                {
                    direction = _lastMoveDirection;
                }

                // Dash Direction
                _dashDirection = new Godot.Vector2(_lastMoveDirection, 0).Normalized();

                // Dashing on and initialize timers
                _isDashing = true;
                _dashTimer = DashDuration;
                _dashCooldownTimer = DashCooldown;
            }
        }

        public void Die()
        {
            GD.Print("Player died");
            // kuolemis animaation käynnistäminen
            // kuolemis äänen soittaminen

            Visible = false;
            _canControl = false;

            ResetPlayer();
        }

        public void RespawnPlayer()
        {
            Show();
            // kameran resetointi
            //_collisionDetector.health = 1; // Reset health
        }

        public async Task HandleDanger()
        {
            GD.Print("Player died");
            Visible = false;
            _canControl = false;
            await ToSignal(GetTree().CreateTimer(1.0f), Timer.SignalName.Timeout);

            ResetPlayer();
        }

        public void ResetPlayer()
        {
            GlobalPosition = GetNode<Node2D>("../RespawnPoint").GlobalPosition; // Assuming RespawnPoint is a Node2D
            Visible = true;
            _canControl = true;
            _jumpCount = 0;
        }
    }
}
