using Godot;
using System;
using System.ComponentModel.DataAnnotations;

public partial class Player : CharacterBody2D
{
    public const float Speed = 200f;
    public const float JumpVelocity = -600f;
    public const float WallJumpVelocity = 1000f;
    public const float WallJumpPush = 1200f;
    public const float WallSlideSpeed = 100f;
    public const float DashSpeed = 500f;
    public const float DashDuration = 0.4f;
    public float Gravity = 1000f;

    private bool _isWallSliding = false;
    private bool _isDashing = false;
    private float _wallJumpDirection = 0;
    private int _jumpCount = 0;
    private Timer _dashTimer;

    [Export]
    public AnimatedSprite2D AnimatedSprite { get; set; } // tarvitaan animaatioita varten


    public override void _Ready()
    {
        _dashTimer = GetNode<Timer>("DashTimer");
        _dashTimer.Timeout += StopDash;
    }

    public override void _PhysicsProcess(double delta)
    {
        Godot.Vector2 velocity = Velocity;
        float inputDirection = Input.GetAxis("Move left", "Move right");

        // Dash logic
        if (Input.IsActionJustPressed("Dash"))
        {
            if (!_isDashing && inputDirection != 0)
            {
                StartDash(inputDirection);
            }
        }

        // Check the direction and apply dash speed
        if (inputDirection == 1 || inputDirection == -1)
        {
            inputDirection = Mathf.Sign(inputDirection); // Varmistetaan, että inputDirection on -1 tai 1
            if (_isDashing)
            {
                velocity.X = inputDirection * DashSpeed;
            }
        }

        // Apply gravity
        if (!IsOnFloor() && !_isWallSliding && !_isDashing)
        {
            velocity.Y += (float)(Gravity * delta);
        }

        BasicMovement(ref velocity, inputDirection);
        Jumping(ref velocity, inputDirection);
        WallSlidingWallJumping(ref velocity, inputDirection, (float)delta);

        // Updates the velocity based on the current state
        Velocity = velocity;
        MoveAndSlide();
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

        // Wall slide
        if (IsOnWall() && !IsOnFloor())
        {
            bool isPressingTowardsWall =
                (GetWallDirection() == 1 && Input.IsActionPressed("Move left")) ||
                (GetWallDirection() == -1 && Input.IsActionPressed("Move right"));

            if (isPressingTowardsWall)
            {
                _isWallSliding = true;
                velocity.Y = Mathf.Min(velocity.Y + Gravity * 0.5f, WallSlideSpeed);
            }
        }

        // Wall jump
        if (_isWallSliding && Input.IsActionJustPressed("Jump"))
        {
            velocity.Y = JumpVelocity;

            int wallDirection = GetWallDirection();

            if (wallDirection != 0)
            {
                velocity.X = wallDirection * WallJumpPush;
            }

            _isWallSliding = false; // Reset wall sliding state after jumping
        }
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
                    return (int)Mathf.Sign(normal.X); // 1 = seinä oikealla, -1 = vasemmalla
                }
            }
        }
        return 0;
    }

    /// <summary>
    /// Starts the dash action for the player.
    /// The dash direction is determined by the input direction.
    /// </summary>
    private void StartDash(float inputDirection)
    {
        if (inputDirection != 0)
        {
            inputDirection = Mathf.Sign(inputDirection); // Varmistetaan, että dash suunta on -1 tai 1
            _isDashing = true;
            _dashTimer.Start(DashDuration);

        }
    }

    /// <summary>
    /// Stops the dash action for the player.
    /// This method is called when the dash timer times out.
    /// </summary>
    private void StopDash()
    {
        _isDashing = false;
    }
}
