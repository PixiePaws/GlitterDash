using Godot;
using System;

namespace UnicornGame
{

public partial class CollisionDetector : Area2D
{
    private bool _isPaused = false;
    //private String _gameOverPath = "";
    //private PackedScene _gameOverScene;
    public int health = 1;

        public override void _Ready()
        {
            //_gameOverScene = ResourceLoader.Load<PackedScene>(_gameOverPath);
            BodyEntered += OnCollisionDetected;
        }

    public void OnCollisionDetected(Node node)
    {
        if (node.IsInGroup("Obstacles"))
        {
            GD.Print("Collided with obstacle!");
            GD.Print("Game over!");
            health = 0;
            InstantiateGameOverScene();
        }

    }

        public void InstantiateGameOverScene()
        {
            GD.Print("InstantiateGameOverScene() was called");
            Node currentScene = GetTree().CurrentScene;
            if (currentScene != null)
            {
                GetTree().ReloadCurrentScene();
            }
            else
            {
                GD.Print("Current scene is null, cannot reload.");
            }
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
