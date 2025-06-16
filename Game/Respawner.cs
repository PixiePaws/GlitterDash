using Godot;
using System;

namespace UnicornGame
{
    public partial class Respawner : Node2D
    {
        [Export] public Node2D RespawnPoint;

        public void RespawnPlayer()
        {
            Player p = GetNode<Player>("PlayerCharacter");
            p.GlobalPosition = RespawnPoint.GlobalPosition;
            p.RespawnPlayer();
        }
    }
}
