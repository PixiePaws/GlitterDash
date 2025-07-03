using Godot;
using System;

namespace UnicornGame
{
    public interface IGameSaver
    {
        string DirectoryPath { get; }
        string FileName { get; }
        string SavePath { get; }
        public void WriteTextToFile(string DirectoryPath, string FileName, string GameData);
    }
}

