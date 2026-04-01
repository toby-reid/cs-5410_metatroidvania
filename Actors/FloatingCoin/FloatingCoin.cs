using Godot;
using System;

namespace Actors
{
    public partial class FloatingCoin : Area2D
    {
        private static class Animation
        {
            public const string Idle = "idle";
            public const string PickUp = "collect";
        }

        [Export]
        private AnimatedSprite2D _sprite;
        [Export]
        private AudioStreamPlayer2D _pickupSound;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            _sprite.Play(Animation.Idle);
            BodyEntered += OnCollide;
        }

        private void OnCollide(Node2D other)
        {
            if (other is Player)
            {
                // PlayerState will handle invalid CoinCount
                ++Global.PlayerState.Instance.CoinCount;
                _sprite.Play(Animation.PickUp);
                _pickupSound.Play();
                _sprite.AnimationFinished += QueueFree;
                BodyEntered -= OnCollide;
            }
        }
    }
}
