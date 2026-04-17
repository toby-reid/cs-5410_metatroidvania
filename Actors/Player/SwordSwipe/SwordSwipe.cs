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
            CheckInitialCollisions();
            BodyEntered += OnBodyEntered;
        }

        private void CheckInitialCollisions()
        {
            foreach (Node2D body in GetOverlappingBodies())
            {
                OnBodyEntered(body);
            }
        }

        private void OnBodyEntered(Node2D other)
        {
            if (other is IEnemy enemy)
            {
                enemy.TakeDamage();
            }
        }
    }
}
