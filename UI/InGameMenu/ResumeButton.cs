using Godot;
using System;

namespace UI
{
    public partial class ResumeButton : Button
    {
        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            Pressed += Global.GameEngine.Instance.ResumeGame;
        }
    }
}
