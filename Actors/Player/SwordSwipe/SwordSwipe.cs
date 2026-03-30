using Godot;
using System;

namespace Actors
{
    public partial class SwordSwipe : Area2D
    {
        [Export]
        private AnimatedSprite2D _sprite;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            _sprite.AnimationFinished += QueueFree;
        }
    }
}
