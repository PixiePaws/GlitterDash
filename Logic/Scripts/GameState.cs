using Godot;
using System;
using System.IO;

namespace UnicornGame
{
    public partial class GameState : Node2D
    {
        public override void _Ready()
        {
        }

        public Godot.Collections.Dictionary<String, Variant> SaveGameState()
        {
            return new Godot.Collections.Dictionary<string, Variant>()
            {
                { "Filename", GetSceneFilePath()},
                { "Parent", GetParent().GetPath()},
                { "PlayerPositionX", GetNode<Player>("/root/")},
                { "PlayerPositionY", GetNode<Player>("/root/")},
                { "SoundVolume", ""},
                { "SoundEffectsVolume", ""},
                { "ResolutionX", ""},
                { "ResolutionY", ""},
                { "EggsCollected", ""},

            };
        }

    }
}