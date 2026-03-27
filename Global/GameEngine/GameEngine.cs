using Godot;
using System;

namespace Global
{
    public partial class GameEngine : Node
    {
        public static GameEngine Instance { get; private set; }

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            Instance = this;
        }

        public void PauseGame()
        {
            GetTree().Paused = true;
        }

        public void ResumeGame()
        {
            GetTree().Paused = false;
        }

        public void TogglePause()
        {
            GetTree().Paused = !GetTree().Paused;
        }
    }
}
