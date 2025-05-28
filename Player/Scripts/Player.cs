using Godot;
using System;

public partial class Player : CharacterBody2D
{
    public const float Speed = 200f;
    public const float JumpVelocity = -400f;
    public float Gravity = 1000f;

    [Export]
    public AnimatedSprite2D AnimatedSprite { get; set; } // tarvitaan animaatioita varten

    public override void _PhysicsProcess(double delta)
    {
        Godot.Vector2 velocity = Velocity;

        // Add the gravity
        // Checks if the player is on the floor
        if (!IsOnFloor())
        {
            velocity.Y += (float)(Gravity * delta);
        }

        // Handle jumping
        // pelkkä maa hyppy
        if (Input.IsActionJustPressed("Jump") && IsOnFloor())
        {
            velocity.Y = JumpVelocity;
        }

        float inputDirection = Input.GetAxis("Move left", "Move right");
        if (inputDirection != 0)
        {
            velocity.X = inputDirection * Speed;
        }
        else
        {
            velocity.X = Mathf.MoveToward(velocity.X, 0, Speed);
        }

        Velocity = velocity;
        MoveAndSlide();
    }
}
