using Godot;
using System;

namespace Environment
{
    public partial class KillZone : Area2D
    {
        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            BodyEntered += OnBodyEntered;
        }

        private void OnBodyEntered(Node2D other)
        {
            if (other is Actors.Player player)
            {
                player.Die();
            }
        }
    }
}
