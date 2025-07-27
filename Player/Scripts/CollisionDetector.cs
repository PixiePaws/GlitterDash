using Godot;
using System;
using System.Threading.Tasks;

namespace UnicornGame
{

    public partial class CollisionDetector : Area2D
    {
        [Export] public Node2D RespawnPoint;
        [Export] public ColorRect Filter;
        [Export] public GoldEggManager _goldeggmanager;
        [Export] public Camera Camera;
        [Export] public Player player;
        private bool _isPaused = false;
        [Export] private Respawner _respawner;
        //private String _gameOverPath = "";
        //private PackedScene _gameOverScene;

        public override void _Ready()
        {
            //_gameOverScene = ResourceLoader.Load<PackedScene>(_gameOverPath);
            BodyEntered += OnCollisionDetected;
            _respawner = GetNode<Respawner>($"../../Respawner");
        }

        /// <summary>
        /// Check if the player collides with an obstacle and then calls for game restart
        /// </summary>
        /// <param name="node"></param>
        public void OnCollisionDetected(Node node)
        {
            if (node.IsInGroup("Obstacles"))
            {
                GD.Print("Collided with obstacle!");
                DieRestart();
            }
        }

        /// <summary>
        /// Puts on the gray filter, calls for player and egg resets
        /// </summary>
        public async void DieRestart()
        {
            player.Die();
            Filter.Visible = true;
            Camera?.ResetCamera("die");
            player?.HandleDanger("dead");
            //_respawner.RespawnPlayer();
            await ToSignal(GetTree().CreateTimer(5f), "timeout"); // tämä 1 pienempi kun resetcameradie timer niin score resettaa samaan aikaan
            _goldeggmanager.ResetEggs();
            /*GD.Print("InstantiateGameOverScene() was called");
            if (_gameOverScene != null)
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
