using Godot;
using System;

using Global;
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

            public const byte AttackSwordSwipeFrame = 2;
        }

        [Export]
        private AnimatedSprite2D _sprite;
        [Export]
        private Timer _attackTimer;
        [Export]
        private PackedScene _swordSwipe;

        public const float Speed = 200f;
        public const float JumpVelocity = -250f;

        private bool _canAnimate = true;
        private SwordSwipe _optSwordSwipe = null;

        public override void _Ready()
        {
            _attackTimer.WaitTime = _sprite.SpriteFrames.GetFrameCount(Animation.Attacking) / _sprite.SpriteFrames.GetAnimationSpeed(Animation.Attacking);
            _attackTimer.OneShot = true;
            _sprite.AnimationChanged += RemoveAnimationArtifacts;
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
                if (PlayerState.Instance.HasUnlock(PlayerState.Progression.Jump))
                {
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
            }
            else if (Input.IsActionJustPressed(Jump) && PlayerState.Instance.HasUnlock(PlayerState.Progression.Jump))
            {
                velocity.Y = JumpVelocity;
            }

            float xDirection = Input.GetAxis(MoveLeft, MoveRight);
            if (
                _attackTimer.IsStopped()
                && (
                    (xDirection < 0 && PlayerState.Instance.HasUnlock(PlayerState.Progression.MoveLeft))
                    || (xDirection > 0 && PlayerState.Instance.HasUnlock(PlayerState.Progression.MoveRight))
                )
            )
            {
                velocity.X = Mathf.MoveToward(Velocity.X, xDirection * Speed, Speed * 10 * (float)delta);
            }
            else
            {
                velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed * 10 * (float)delta);
            }

            Velocity = velocity;
            if (_canAnimate)
            {
                SetAnimation();
            }

            KinematicCollision2D collision = MoveAndCollide(Velocity * (float)delta);
            if (collision != null && collision.GetCollider() is IEnemy)
            {
                TakeDamage();
            }
        }

        public override void _Input(InputEvent @event)
        {
            if (@event.IsActionPressed(Attack))
            {
                TryAttack();
            }
        }

        private void TryAttack()
        {
            if (_attackTimer.IsStopped() && PlayerState.Instance.HasUnlock(PlayerState.Progression.BaseAttack))
            {
                // TODO (#31): create "sword" object for collisions
                _sprite.Play(Animation.Attacking);
                _sprite.FrameChanged += CheckAttackFrame;
                _attackTimer.Start();
                AddSpriteTriggers();
            }
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
            _canAnimate = true;
            SetAnimation(); // required to avoid a single frame of weird offset for Attack animation
        }

        private void RemoveAnimationArtifacts()
        {
            if (_optSwordSwipe != null)
            {
                if (IsInstanceValid(_optSwordSwipe))
                {
                    // This is mainly useful for if we take damage while attacking;
                    // attack should be aborted immediately
                    _optSwordSwipe.QueueFree();
                }
                _optSwordSwipe = null; // let C# descope the object; Godot already has
            }
        }

        private void CheckAttackFrame()
        {
            if (_sprite.Animation == Animation.Attacking)
            {
                if (_sprite.Frame == Animation.AttackSwordSwipeFrame)
                {
                    _optSwordSwipe = _swordSwipe.Instantiate<SwordSwipe>();
                    _optSwordSwipe.Scale = _sprite.FlipH ? new(-1, 1) : new(1, 1);
                    _optSwordSwipe.Sprite.SpriteFrames.SetAnimationSpeed(Animation.Attacking, _sprite.SpriteFrames.GetAnimationSpeed(Animation.Attacking));
                    _optSwordSwipe.Sprite.Play(Animation.Attacking);
                    // Keep default offset; SwordSwipe should be offset internally
                    AddChild(_optSwordSwipe);
                    _sprite.FrameChanged -= CheckAttackFrame;
                }
            }
            else
            {
                _sprite.FrameChanged -= CheckAttackFrame;
            }
        }
    }
}
