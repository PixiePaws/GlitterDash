using Godot;
using System;
using UnicornGame;

public abstract partial class GameLevels : Node2D
{
    protected GameManager _gameManager;
    protected GameState _gameState;
    private PackedScene _gameManagerScene;
    private string _gameManagerScenePath;
    
    [Export] private SaveFormat _saveFormat;

    public enum SaveFormat
    {
        Xml,
        Json,
        None
    }
    public SaveFormat CurrentSaveFormat
    {
        get { return _saveFormat; }
    }
    public void InstantiateGameManager()
    {
        _gameManagerScenePath = ("res://Logic/Scenes/GameManager.tscn");
        _gameManagerScene = ResourceLoader.Load<PackedScene>(_gameManagerScenePath);
        GD.Print("Game manager scene loaded");
        var GameManager = _gameManagerScene.Instantiate();
        _gameManager = (GameManager)GameManager;
        GD.Print("Game manager scene instantiated");
        AddChild(_gameManager);
        if (_gameManager != null)
        {
            GD.Print("_gameManager added to tree successfully");
        }
        else
        {
            GD.Print("_gameManager could not be added to tree");
        }
        _gameState = new GameState();
    }
}
