using Godot;
using System;

namespace Global
{
    public partial class PauseManager : Node
    {
        public static PauseManager Instance { get; private set; }

        public event Action<bool> OnTogglePause;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            Instance = this;
        }

        public void PauseGame()
        {
            if (!GetTree().Paused)
            {
                TogglePause();
            }
        }

        public void ResumeGame()
        {
            if (GetTree().Paused)
            {
                TogglePause();
            }
        }

        public void TogglePause()
        {
            bool isNowPaused = !GetTree().Paused;
            GetTree().Paused = isNowPaused;
            OnTogglePause(isNowPaused);
        }
    }
}
