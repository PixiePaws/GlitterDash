using Godot;
using System;
using System.Collections.Generic;

namespace UnicornGame
{
    public partial class GoldEggManager : Node
    {
        [Export] private Respawner _respawner;
        [Export] public PackedScene GoldEggScene;

        private List<Vector2> _originalPositions = new();

        public override void _Ready()
        {
            foreach (Node child in GetChildren())
            {
                if (child is GoldEgg goldEgg)
                {
                    _originalPositions.Add(goldEgg.GlobalPosition);
                }
            }
        }

        /// <summary>
        /// Resets collected egg to the places they were and calls for score reset
        /// </summary>
        public async void ResetEggs()
        {
            await ToSignal(GetTree().CreateTimer(1.0f), Timer.SignalName.Timeout);

            _respawner.ResetScore();

            // Make new eggs at the original positions
            foreach (Vector2 pos in _originalPositions)
            {
                bool hasEgg = false;

                foreach (Node child in GetChildren())
                {
                    if (child is GoldEgg existingEgg)
                    {
                        if (existingEgg.GlobalPosition.DistanceTo(pos) < 1.0f)
                        {
                            hasEgg = true;
                            break;
                        }
                    }
                }

                if (!hasEgg)
                {
                    GoldEgg newEgg = GoldEggScene.Instantiate<GoldEgg>();
                    newEgg.GlobalPosition = pos;
                    AddChild(newEgg);
                }
            }
        }
    }
}