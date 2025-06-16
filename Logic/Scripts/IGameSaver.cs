using Godot;
using System;

namespace UnicornGame
{
    public interface IGameSaver
    {
        public void WriteTextToFile(string DirectoryPath, string FileName, string GameData);
    }
}

