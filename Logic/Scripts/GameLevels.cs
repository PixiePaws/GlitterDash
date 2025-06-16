using Godot;
using System;
using UnicornGame;

public abstract partial class GameLevels : Node2D
{
    protected GameManager _gameManager;
    public override void _Ready()
    {
        _gameManager = new GameManager();
    }
}
