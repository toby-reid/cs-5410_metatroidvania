using Godot;
using System;

namespace UI
{
    public partial class InGameMenu : CanvasLayer
    {
        public override void _Ready()
        {
            Visible = GetTree().Paused;
            Global.PauseManager.Instance.OnTogglePause += OnPause;
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
                    if (Actors.Player.CanMove)
                    {
                        Global.PauseManager.Instance.TogglePause();
                    }
                }
            }
        }
    }
}
