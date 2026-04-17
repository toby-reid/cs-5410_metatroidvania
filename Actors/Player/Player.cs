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
            public const string Dying = "death";

            public const byte AttackSwordSwipeFrame = 2;
        }

        [ExportGroup("Set in Object")]
        [Export] private AnimatedSprite2D _sprite;
        [Export] private PackedScene _swordSwipe;
        [Export] private RayCast2D _floorDetector;

        public const float Speed = 200f;
        public const float JumpVelocity = -250f;

        private readonly Timer _attackTimer = new();
        private readonly Timer _damageTimer = new();

        private bool _canAnimate = true;
        private SwordSwipe _optSwordSwipe = null;

        public override void _Ready()
        {
            SetAnimationTimer(_attackTimer, Animation.Attacking);
            SetAnimationTimer(_damageTimer, Animation.TakingDamage);
            _sprite.AnimationChanged += RemoveAnimationArtifacts;
            _sprite.Play(Animation.Idle);
        }

        public override void _PhysicsProcess(double delta)
        {
            Vector2 velocity = Velocity;

            bool canMoveFreely = _attackTimer.IsStopped() && _damageTimer.IsStopped();
            GD.Print(canMoveFreely);
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
            else if (canMoveFreely)
            {
                if (Input.IsActionJustPressed(Jump) && PlayerState.Instance.HasUnlock(PlayerState.Progression.Jump))
                {
                    velocity.Y = JumpVelocity;
                }
                else if (Input.IsActionJustPressed(MoveDown) && PlayerState.Instance.HasUnlock(PlayerState.Progression.DropThroughPlatform))
                {
                    if (_floorDetector.IsColliding() && _floorDetector.GetCollider() is Environment.OneWayPlatform)
                    {
                        // Shift down slightly
                        // This is done after normal gravity, so it takes slightly longer to fall through
                        Position += Vector2.Down;
                    }
                }
            }

            float xDirection = Input.GetAxis(MoveLeft, MoveRight);
            if (
                canMoveFreely
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
            if (canMoveFreely)
            {
                SetAnimation();
            }
            MoveAndSlide();

            int collisionCount = GetSlideCollisionCount();
            for (int i = 0; i < collisionCount; ++i)
            {
                if (GetSlideCollision(i) is KinematicCollision2D collision && collision.GetCollider() is IEnemy)
                {
                    TakeDamage();
                    break;
                }
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
            }
        }

        public void TakeDamage()
        {
            // TODO: add sound effects
            if (--PlayerState.Instance.HP == 0)
            {
                Die();
            }
            else
            {
                _sprite.Play(Animation.TakingDamage);
                _damageTimer.Start();
            }
        }

        public void Die()
        {
            // TODO: Play death sfx
            Timer deathTimer = new()
            {
                WaitTime = 5,
                ProcessMode = ProcessModeEnum.WhenPaused,
                Autostart = true
            };
            deathTimer.Timeout += () => SceneChanger.Instance.GoToGameOver("Your death was in vain.");
            AddChild(deathTimer);
            PauseManager.Instance.PauseGame();
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

        private void SetAnimationTimer(Timer timer, string animation)
        {
            AddChild(timer);
            timer.WaitTime = _sprite.SpriteFrames.GetFrameCount(animation) / _sprite.SpriteFrames.GetAnimationSpeed(animation);
            timer.OneShot = true;
        }
    }
}
