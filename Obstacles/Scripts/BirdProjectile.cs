using Godot;
using System;

namespace UnicornGame
{
    public partial class BirdProjectile : RigidBody2D
    {
        [Export] private Vector2 _impulseVector = new Vector2(-50.0f, 0.0f);
        [Export] private Area2D _collisionDetector;
        private string _spriteWingsUpPath;
        private string _spriteWingsDownPath;
        private Sprite2D _sprite;
        private Texture2D _spriteWingsUp;
        private Texture2D _spriteWingsDown;
        public override void _Ready()
        {
            //_impulseVector = new Vector2(-50.0f, 0.0f);
            _collisionDetector = GetNode<Area2D>("CollisionDetector");
            _collisionDetector.BodyEntered += OnCollisionDetected;
            _sprite = GetNode<Sprite2D>("Sprite2D");
            if (_sprite != null)
            {
                GD.Print("Succesfully got sprite reference in bird projectile");
            }
            GD.Print($"Bird projectile texture: {_sprite.Texture}");
            _spriteWingsUpPath = "res://Art/Obstacles/Bird_WingUp.png";
            _spriteWingsDownPath = "res://Art/Obstacles/Bird_WingDown.png";
            _spriteWingsUp = ResourceLoader.Load<Texture2D>(_spriteWingsUpPath);
            _spriteWingsDown = ResourceLoader.Load<Texture2D>(_spriteWingsDownPath);
            _sprite.Texture = _spriteWingsUp;
            ApplyCentralImpulse(_impulseVector);
        }
        public override void _Process(double delta)
        {
            AlternateSprite();
        }

        public void OnCollisionDetected(Node node)
        {
            QueueFree();
        }
        public void AlternateSprite()
        {
            //GD.Print($"bird texture wings up {_sprite.Texture == _spriteWingsUp}");
            //GD.Print($"bird texture wings down {_sprite.Texture == _spriteWingsDown}");
            
            if (_sprite.Texture == _spriteWingsUp)
            {
                _sprite.Texture = _spriteWingsDown;
            }
            else
            {
                _sprite.Texture = _spriteWingsUp;
            }
        }
        public void CreateTimer()
        {
            Timer AlternatingTimer = new Timer();
        }
    }
}

