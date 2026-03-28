using Godot;
using System;

using static Global.Constants.InputMap;

namespace Actors
{
    public partial class Player : CharacterBody2D
    {
        private static class Animation
        {
            public const string Idle = "idle";
            public const string Running = "run";
            public const string Jumping = "jump";
            public const string Falling = "fall";
            public const string Attacking = "attack";
            public const string TakingDamage = "take_damage";

            public static readonly Vector2 AttackingOffset = new(8, 0);
        }

        [Export]
        private AnimatedSprite2D _sprite;

        public const float Speed = 300.0f;
        public const float JumpVelocity = -400.0f;

        private bool _canAnimate = true;

        public override void _Ready()
        {
            _sprite.Play(Animation.Idle);
        }

        public override void _PhysicsProcess(double delta)
        {
            Vector2 velocity = Velocity;

            bool isOnFloor = IsOnFloor();
            if (!isOnFloor)
            {
                Vector2 gravity = GetGravity() * (float)delta;
                velocity += gravity;
                if (Input.IsActionPressed(Jump))
                {
                    // Halve gravity
                    velocity -= gravity / 2;
                }
                else if (Input.IsActionPressed(MoveDown))
                {
                    // Double gravity
                    velocity += gravity;
                }
            }
            else if (Input.IsActionJustPressed(Jump) && Global.PlayerState.Instance.HasAbility(Global.PlayerState.Progression.Jump))
            {
                velocity.Y = JumpVelocity;
            }

            float xDirection = Input.GetAxis(MoveLeft, MoveRight);
            if (
                (xDirection < 0 && Global.PlayerState.Instance.HasAbility(Global.PlayerState.Progression.MoveLeft))
                || (xDirection > 0 && Global.PlayerState.Instance.HasAbility(Global.PlayerState.Progression.MoveRight))
            )
            {
                velocity.X = xDirection * Speed;
            }
            else
            {
                velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
            }

            Velocity = velocity;
            if (_canAnimate)
            {
                SetAnimation();
            }
            MoveAndSlide();
        }

        public override void _Input(InputEvent @event)
        {
            if (@event.IsActionPressed(Global.Constants.InputMap.Attack))
            {
                Attack();
            }
        }

        private void Attack()
        {
            // TODO (#31): create "sword" object for collisions
            _sprite.Play(Animation.Attacking);
            _sprite.Offset = Animation.AttackingOffset;
            AddSpriteTriggers();
        }

        public void TakeDamage()
        {
            // TODO: add sound effects
            _sprite.Play(Animation.TakingDamage);
            AddSpriteTriggers();
        }

        private void SetAnimation()
        {
            if (Velocity.X != 0)
            {
                _sprite.FlipH = Velocity.X < 0;
            }
            string animation = _sprite.Animation;
            if (IsOnFloor())
            {
                animation = (Velocity.X == 0) ? Animation.Idle : Animation.Running;
            }
            else
            {
                animation = (Velocity.Y > 0) ? Animation.Falling : Animation.Jumping;
            }
            _sprite.Play(animation);
        }

        private void AddSpriteTriggers()
        {
            _sprite.AnimationFinished += ResetSpriteTriggers;
            _canAnimate = false;
        }

        private void ResetSpriteTriggers()
        {
            _sprite.AnimationFinished -= ResetSpriteTriggers;
            _sprite.Offset = Vector2.Zero;
            _canAnimate = true;
            SetAnimation(); // required to avoid a single frame of weird offset for Attack animation
        }
    }
}
