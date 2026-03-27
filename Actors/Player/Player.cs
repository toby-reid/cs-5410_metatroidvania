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
            public const string Attacking = "attack";
            public const string TakingDamage = "take_damage";
        }

        [Export]
        private AnimatedSprite2D _sprite;

        public const float Speed = 300.0f;
        public const float JumpVelocity = -400.0f;

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
                _sprite.Play(Animation.Jumping);
            }

            float xDirection = Input.GetAxis(MoveLeft, MoveRight);
            if (
                (xDirection < 0 && Global.PlayerState.Instance.HasAbility(Global.PlayerState.Progression.MoveLeft))
                || (xDirection > 0 && Global.PlayerState.Instance.HasAbility(Global.PlayerState.Progression.MoveRight))
            )
            {
                velocity.X = xDirection * Speed;
                if (isOnFloor)
                {
                    _sprite.Play(Animation.Running);
                }
            }
            else
            {
                velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
                if (isOnFloor && velocity.X == 0)
                {
                    _sprite.Play(Animation.Idle);
                }
            }

            Velocity = velocity;
            if (Velocity.X < 0)
            {
                _sprite.FlipH = true;
            }
            MoveAndSlide();
        }
    }
}
