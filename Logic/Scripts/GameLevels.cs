using Godot;
using System;
using UnicornGame;

public abstract partial class GameLevels : Node2D
{
    protected GameManager _gameManager;
    protected GameState _gameState;
    private PackedScene _gameManagerScene;
    private string _gameManagerScenePath;
    protected string _loadedSaveFile;
    protected bool _levelCompleted = false;

    [Export] private SaveFormat _saveFormat = SaveFormat.Json;

    public enum SaveFormat
    {
        Xml,
        Json,
        None
    }
    public bool CurrentLevelCompleted
    {
        get { return _levelCompleted; }
        set { _levelCompleted = value; }
    }
    public GameState CurrentGameState
    {
        get { return _gameState; }
    }
    public SaveFormat CurrentSaveFormat
    {
        get { return _saveFormat; }
    }
    public GameManager GameManager
    {
        get { return _gameManager; }
    }
    public string LoadedSaveFile
    {
        get { return _loadedSaveFile; }
        set { _loadedSaveFile = value; }
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
        AddChild(_gameState);
        if (_gameState != null)
        {
            GD.Print("_gameState in GameLevels successfully created");
        }
        else
        {
            GD.Print("_gameState in GameLevels is null!");
        }
    }
    public void LoadSave(string FileName)
    {

    }
}
