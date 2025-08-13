using Godot;
using System;

namespace UnicornGame
{

    public partial class Level1 : GameLevels
    {
        [Export] private Node2D RespawnPoint;


        public override void _Ready()
        {
            GD.Print("Level1 _Ready()");
            GD.Print($"parent path: {GetParent().GetPath()}");
            GD.Print($"scene file path: {GetSceneFilePath()}");
            InstantiateGameManager();
        }
    }
}