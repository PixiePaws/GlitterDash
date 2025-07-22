using Godot;
using System;

namespace UnicornGame
{
    public partial class BirdProjectile : RigidBody2D
    {
        [Export] private Vector2 _impulseVector = new Vector2(-50.0f, 0.0f);
        [Export] private Area2D _collisionDetector;
        public override void _Ready()
        {
            //_impulseVector = new Vector2(-50.0f, 0.0f);
            _collisionDetector = GetNode<Area2D>("CollisionDetector");
            _collisionDetector.BodyEntered += OnCollisionDetected;
            ApplyCentralImpulse(_impulseVector);
        }
        public void OnCollisionDetected(Node node)
        {
            QueueFree();
        }
    }
}

