using Godot;
using System;

namespace UnicornGame
{
    public partial class BirdCannon : StaticBody2D
    {
        private string _birdProjectileScenePath;
        private PackedScene _birdProjectileScene;
        public override void _Ready()
        {
            _birdProjectileScenePath = "res://Obstacles/Scenes/BirdProjectile.tscn";
            _birdProjectileScene = ResourceLoader.Load<PackedScene>(_birdProjectileScenePath);

        }
        public void ShootBirdProjectile()
        {
            BirdProjectile Bird = (BirdProjectile)_birdProjectileScene.Instantiate();
        }
    }
}

