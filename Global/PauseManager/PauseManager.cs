using Godot;
using System;

namespace Global
{
    public partial class PauseManager : Node
    {
        public static PauseManager Instance { get; private set; }

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                GD.PrintErr("Attempted to create another PauseManager when one was already in use");
            }
        }

        public override void _ExitTree()
        {
            Instance = null;
        }

        public bool IsGamePaused()
        {
            return GetTree().Paused;
        }

        public bool PauseGame()
        {
            if (!IsGamePaused())
            {
                GetTree().Paused = true;
                return true;
            }
            GD.PrintErr("Attempted to pause game when it was already paused");
            return false;
        }

        public bool ResumeGame()
        {
            if (IsGamePaused())
            {
                GetTree().Paused = false;
                return true;
            }
            GD.PrintErr("Attempted to resume game when it was not paused");
            return false;
        }
    }
}
