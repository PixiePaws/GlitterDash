using Godot;
using System;

namespace UnicornGame
{
    public partial class Tutorial : Node
    {
        private bool _hasTriggered = false;

        public override async void _Ready()
        {
            GD.Print("Delaying signal connection");
            await ToSignal(GetTree(), "process_frame");
            Callable.From(ConnectSignal).CallDeferred();
        }

        private void ConnectSignal()
        {
            GD.Print("Signal connected AFTER scene is ready");
            GetNode<Area2D>("Tutorial1Text/Tutorial1").BodyEntered += OnBodyEntered;
        }

        private void OnBodyEntered(Node body)
        {
            GD.Print("Body entered in tutorial1");
            GetNode<Label>("Tutorial1Text").Visible = true;
            GetNode<Area2D>("Tutorial1Text/Tutorial1").BodyEntered -= OnBodyEntered; //stop listening for the signal
            GetNode<AnimationPlayer>("Tutorial1Text/AnimationPlayer").Play("Tutorial1TextAnimation");
        }

        private void PauseGame()
        {
            // pysäytä pelaaja ja peli siihen asti, kunnes pelaaja painaa haluttua nappulaa
        }
    }
}
