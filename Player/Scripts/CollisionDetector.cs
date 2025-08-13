using Godot;
using System;
using System.Threading.Tasks;

namespace UnicornGame
{

    public partial class CollisionDetector : Area2D
    {
        [Export] public Node2D RespawnPoint;
        [Export] public ColorRect _filter;
        [Export] public GoldEggManager _goldeggmanager;
        [Export] public Camera Camera;
        [Export] public Player _player;
        private AudioManager _audioManager;
        private AudioStream walkSound;
        private bool _isPaused = false;
        [Export] private Respawner _respawner;
        //private String _gameOverPath = "";
        //private PackedScene _gameOverScene;

        public override void _Ready()
        {
            
            var ParentPlayerPath = GetParent().GetPath();
			_player = GetNode<Player>($"{ParentPlayerPath}");
            var ParentLevelPath = _player.GetParent().GetPath();
            _filter = GetNode<ColorRect>($"{ParentLevelPath}/BlackWhite");
            _goldeggmanager = GetNode<GoldEggManager>($"{ParentLevelPath}/GoldEggs");

            _audioManager = GetNode<AudioManager>("/root/AudioManager");
            walkSound = GD.Load<AudioStream>("res://Audio/Sfx/walking-on-grass-quickly.mp3");
            //GD.Print("Succesfully got audio manager reference");

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
                AudioManager.StopSound(walkSound);
                _audioManager.PlayHitSound(node);
                DieRestart();
            }
        }

        /// <summary>
        /// Puts on the gray _filter, calls for player and egg resets
        /// </summary>
        public async void DieRestart()
        {
            _player.Die();
            _filter.Visible = true;
            Camera?.ResetCamera("die");
            _player?.HandleDanger("dead");
            //_respawner.RespawnPlayer();
            await ToSignal(GetTree().CreateTimer(4f), "timeout"); // tämä 1 pienempi kun resetcameradie timer niin score resettaa samaan aikaan
            _goldeggmanager.ResetEggs();
        }
    }
}
