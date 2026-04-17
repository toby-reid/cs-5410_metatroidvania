using Godot;
using System;


namespace Actors
{
    public partial class Turnip : Node, IEnemy
    {
        private static class Animation
        {
            public const string Idle = "idle";
            public const string TakingDamage = "take_damage";
            public const string Death = "death";
        }

        [Export]
        private AnimatedSprite2D _sprite;

        public void Die()
        {
            _sprite.Play(Animation.Death);
        }

        public void AdvanceAnimation()
        {
            if (_sprite.Animation == Animation.Death) {
                QueueFree();
            }
        }

        public override void _Ready()
        {
            _sprite.Play(Animation.Idle);
            _sprite.AnimationFinished += AdvanceAnimation;
        }
    }
}
