using Godot;
using System;

namespace Actors
{
    public partial class SwordSwipe : Area2D
    {
        [Export]
        public AnimatedSprite2D Sprite { get; private set; }

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            Sprite.AnimationFinished += QueueFree;
        }
    }
}
