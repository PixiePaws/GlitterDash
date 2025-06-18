using Godot;
using System;

namespace UnicornGame
{

    public partial class CollisionDetector : Area2D
    {
        [Export] public Node2D RespawnPoint;
        [Export] public ColorRect Fliter;
        private bool _isPaused = false;
        private Respawner _respawner;
        //private String _gameOverPath = "";
        //private PackedScene _gameOverScene;

        public override void _Ready()
        {
            //_gameOverScene = ResourceLoader.Load<PackedScene>(_gameOverPath);
            BodyEntered += OnCollisionDetected;
            _respawner = GetNode<Respawner>($"../../Respawner");
        }

        public void OnCollisionDetected(Node node)
        {
            if (node.IsInGroup("Obstacles"))
            {
                GD.Print("Collided with obstacle!");
                GD.Print("Game over!");
                TurnOnFliter();
            }
        }

        public void TurnOnFliter()
        {
            Fliter.Visible = true;
            _respawner.RespawnPlayer();
            GD.Print("InstantiateGameOverScene() was called");
            /*if (_gameOverScene != null)
            {
                Node GameOverPanel = _gameOverScene.Instantiate();
                AddChild(GameOverPanel);
                GD.Print("Game over scene was instantiated");
            }
            else
            {
                GD.Print("Game over scene not found!");
            }*/
        }
    }
}
