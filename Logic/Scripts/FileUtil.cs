using Godot;
using System;

namespace UnicornGame
{
    public static class FileUtil
    {
        public static int GetSaveFilesAmount(string DirectoryPath)
        {
            int SaveFilesAmount = DirAccess.GetFilesAt(DirectoryPath).Length;
            return SaveFilesAmount;
        }
    }
}

