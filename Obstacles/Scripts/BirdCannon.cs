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
            if (_birdProjectileScene != null)
            {
                GD.Print("Succesfully got bird projectile scene");
            }
            ShootBirdProjectile();
        }
        public void ShootBirdProjectile()
        {
            BirdProjectile Bird = (BirdProjectile)_birdProjectileScene.Instantiate();
            AddChild(Bird);
            if (Bird != null)
            {
                GD.Print("Successfully instantiated bird projectile");
            }
        }
    }
}

