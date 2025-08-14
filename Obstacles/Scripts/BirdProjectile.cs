using Godot;
using System;

namespace UnicornGame
{
    public partial class BirdProjectile : RigidBody2D
    {
        [Export] private Vector2 _impulseVector = new Vector2(-50.0f, 0.0f);
        [Export] private Area2D _collisionDetector;
        [Export] private Area2D _soundDetector;
        private string _spriteWingsUpPath;
        private string _spriteWingsDownPath;
        private Sprite2D _sprite;
        private Texture2D _spriteWingsUp;
        private Texture2D _spriteWingsDown;
        private AudioManager _audioManager;
        [Export] private float _timerWaitTime;
        public override void _Ready()
        {
            _audioManager = GetNode<AudioManager>("/root/AudioManager");
            _collisionDetector = GetNode<Area2D>("CollisionDetector");
            _collisionDetector.BodyEntered += OnCollisionDetected;
            _soundDetector = GetNode<Area2D>("SoundDetector");
            _soundDetector.BodyEntered += OnSoundDetected;
            _sprite = GetNode<Sprite2D>("Sprite2D");
            if (_sprite != null)
            {
                //GD.Print("Succesfully got sprite reference in bird projectile");
            }
            //GD.Print($"Bird projectile texture: {_sprite.Texture}");
            _spriteWingsUpPath = "res://Art/Obstacles/Bird_WingUp.png";
            _spriteWingsDownPath = "res://Art/Obstacles/Bird_WingDown.png";
            _spriteWingsUp = ResourceLoader.Load<Texture2D>(_spriteWingsUpPath);
            _spriteWingsDown = ResourceLoader.Load<Texture2D>(_spriteWingsDownPath);
            SetSpriteDirection();
            _sprite.Texture = _spriteWingsUp;
            ApplyCentralImpulse(_impulseVector);
            Timer SpriteTimer = new Timer();
            AddChild(SpriteTimer);
            SpriteTimer.WaitTime = 0.1f;
            SpriteTimer.Timeout += AlternateSprite;
            SpriteTimer.OneShot = false;
            SpriteTimer.Start();
        }
        public override void _Process(double delta)
        {
            //AlternateSprite();
        }
        public Vector2 ImpulseVector
        {
            get { return _impulseVector; }
            set { _impulseVector = value; }
        }
        public void OnCollisionDetected(Node node)
        {
            //GD.Print($"BirdProjectile OnCollisionDetected was triggered by {node}, name: {node.Name}, parent: {node.GetParent()}");
            QueueFree();
        }
        public void OnSoundDetected(Node node)
        {
            //GD.Print($"OnCollisionDetected sound in BirdProjectile.cs was triggered by {node}");
            if (node is Player player)
            {
                //GD.Print("Playing _birdProjectileFlyBy");
                _audioManager._birdProjectileFlyBy.Play();
            }
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
        public void SetSpriteDirection()
        {
            if (_impulseVector.X < 0)
            {
                _sprite.FlipH = true;
            }
            else
            {
                _sprite.FlipH = false;
            }
        }
        public void TimeoutPrint()
        {
            GD.Print("Timeout");
        }
    }
}

