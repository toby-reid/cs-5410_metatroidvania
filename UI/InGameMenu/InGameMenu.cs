using Godot;
using System;

using Global;

namespace UI
{
    public partial class InGameMenu : CanvasLayer
    {
        [Export]
        private BaseButton _resumeButton;

        private bool _isMyPause = false;

        public override void _Ready()
        {
            Visible = false;
            PauseManager.Instance.OnTogglePause += OnPause;
            _resumeButton.Pressed += ResumeGame;
        }

        public void OnPause(bool isPaused)
        {
            Visible = isPaused;
        }

        // Temporary inclusion. Will need to determine what circumstances let you open the pause menu
        public override void _Input(InputEvent @event)
        {
            if (@event is InputEventKey keyEvent)
            {
                if (keyEvent.Pressed && keyEvent.Keycode == Key.Escape)
                {
                    if (PauseManager.IsGamePaused())
                    {
                        if (_isMyPause)
                        {
                            ResumeGame();
                        }
                    }
                    else
                    {
                        PauseGame();
                    }
                }
            }
        }

        private void ResumeGame()
        {
            PauseManager.ResumeGame();
            _isMyPause = false;
        }

        private void PauseGame()
        {
            PauseManager.PauseGame();
            _isMyPause = true;
        }
    }
}
