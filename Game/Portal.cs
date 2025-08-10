using Godot;
using System;

namespace UnicornGame
{
    public partial class Portal : Area2D
    {
        [Export] public string _level2ScenePath = "res://Game/Level2/Scenes/Level2.tscn";
        [Export] public string _level3ScenePath = "res://Game/Level3/Scenes/Level3.tscn";
        [Export] public string _level4ScenePath = "res://Game/Level4/Scenes/Level4.tscn";
        [Export] public string _level5ScenePath = "res://Game/Level5/Scenes/Level5.tscn";
        [Export] public string _level6ScenePath = "res://Game/Level6/Scenes/Level6.tscn";

        [Export] public string _levelsCompletedScenePath = "res://Game/LevelsCompleted.tscn";

		private AudioManager _audioManager;
		private AudioStream portalSound;

        private string _nextLevelPath = "";
        
		public override void _Ready()
        {
			_audioManager = GetNode<AudioManager>("/root/AudioManager");
			portalSound = GD.Load<AudioStream>("res://Audio/Sfx/fast-warp-in.wav");

            BodyEntered += OnBodyEntered; // Signal to detect when a body enters the portal area
        }

        /// <summary>
        /// This method is called when a body enters the portal area.
        /// </summary>
        /// <param name="body"></param>
        public void OnBodyEntered(Node body)
        {
            if (body is Player)
            {
                //GD.Print("Player entered the portal, transferring to Level 2.");
                CallDeferred(nameof(PortalTransfer));
            }
        }

        /// <summary>
        /// Transfers the player to the next level based on the current scene.
        /// This method is called when the player enters the portal.
        /// It checks the current scene and determines the next level to load.
        /// </summary>
        public void PortalTransfer()
        {
            GameLevels currentScene = (GameLevels)GetTree().CurrentScene;

            if (currentScene == null)
            {
                GD.PrintErr("[ERROR] No active scene found!");
                return;
            }
            currentScene.CurrentLevelCompleted = true;
            var ParentName = GetParent().Name;
            GameState CurrentState = currentScene.CurrentGameState;
                if (CurrentState != null)
                {
                    GD.Print("Got CurrentGameState reference");
                }
                else
                {
                    GD.Print("Could not get CurrentGameState reference");
                }
                Godot.Collections.Dictionary<string, Variant> SaveData = CurrentState.SaveGameState();
                string JsonString = Json.Stringify(SaveData);
                IGameSaver GameSaver = GetNode<GameLevels>($"/root/{ParentName}/").GameManager.GameSaver;
                if (GameSaver != null)
                {
                    GD.Print("Got Game Saver reference succesfully");    
                }
                //GD.Print(GameSaver.DirectoryPath);
                //GD.Print(GameSaver.FileName);
                //GD.Print(JsonString);
                GameSaver.WriteTextToFile(GameSaver.DirectoryPath, GameSaver.FileName, JsonString);

            int nextLevel = 0;

            // Choosing the next level based on the current scene name.
            switch (currentScene.Name)
            {
                case "Level1":
                    nextLevel = 2;
                    break;

                case "Level2":
                    nextLevel = 3;
                    break;

                case "Level3":
                    nextLevel = 4;
                    break;

                case "Level4":
                    nextLevel = 5;
                    break;
                case "Level5":
                    nextLevel = 6;
                    break;
                case "Level6":
                    nextLevel = 7;
                    break;

                default:
                    GD.PrintErr("[ERROR] No active level found!");
                    return;
            }

            // Load and change the scene
            _nextLevelPath = GetLevelScenePath(nextLevel);

            PackedScene nextSceneLevel = (PackedScene)GD.Load(_nextLevelPath);

            AudioManager.PlaySound2(portalSound);
            GetTree().ChangeSceneToPacked(nextSceneLevel);
        }

        /// <summary>
		/// Returns the scene path for the next level based on the current level.
		/// </summary>
		/// <param name="nextLevel"></param>
		private string GetLevelScenePath(int nextLevel)
		{
			return nextLevel switch
            {
                2 => _level2ScenePath,
                3 => _level3ScenePath,
                4 => _level4ScenePath,
                5 => _level5ScenePath,
                6 => _level6ScenePath,
                7 => _levelsCompletedScenePath,
                _ => string.Empty
            };
		}
	}
}