using Godot;
using System;

namespace UnicornGame
{
    public partial class BirdCannon : StaticBody2D
    {
        [Export] private Vector2 _impulseVector = new Vector2(-500.0f, 0.0f);
        private string _birdProjectileScenePath;
        private PackedScene _birdProjectileScene;
        [Export] bool _shootTimerOneShot = false;
        [Export] float _shootTimerWaitTime = 5.0f;
        private Timer _shootTimer;
        public override void _Ready()
        {
            _birdProjectileScenePath = "res://Obstacles/Scenes/BirdProjectile.tscn";
            _birdProjectileScene = ResourceLoader.Load<PackedScene>(_birdProjectileScenePath);
            if (_birdProjectileScene != null)
            {
                GD.Print("Succesfully got bird projectile scene");
            }
            _shootTimer = new Timer();
            AddChild(_shootTimer);
            _shootTimer.OneShot = _shootTimerOneShot;
            _shootTimer.WaitTime = _shootTimerWaitTime;
            _shootTimer.Timeout += OnTimerTimeOut;
            _shootTimer.Start();

        }
        public void ShootBirdProjectile()
        {
            BirdProjectile Bird = (BirdProjectile)_birdProjectileScene.Instantiate();
            Bird.ImpulseVector = _impulseVector;
            AddChild(Bird);
            if (Bird != null)
            {
                GD.Print("Successfully instantiated bird projectile");
            }
        }
        public void StartTimer()
        {
            _shootTimer.Start();
        }
        public void OnTimerTimeOut()
        {
            ShootBirdProjectile();
        }
    }
}

