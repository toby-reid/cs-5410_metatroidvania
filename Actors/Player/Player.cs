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
        [Export] private CollisionShape2D _collisionShape;
        [Export] private AudioStreamPlayer2D _healSfx;

        public const float Speed = 200f;
        public const float JumpVelocity = -250f;
        private const float HealTimeSec = 3f;

        private readonly Timer _damageImmunity = new();
        private readonly Timer _forcedMovement = new();

        private bool _canAnimate = true;
        private SwordSwipe _optSwordSwipe = null;
        private Timer _optHealTimer = null;

        public override void _Ready()
        {
            AddChild(_damageImmunity);
            AddChild(_forcedMovement);
            SetAnimationTimer(_damageImmunity, Animation.TakingDamage);
            _forcedMovement.OneShot = true;
            _sprite.AnimationChanged += RemoveAnimationArtifacts;
            _sprite.Play(Animation.Idle);
            PlayerState.Instance.OnHPChange += OnHPChange;
        }

        public override void _ExitTree()
        {
            PlayerState.Instance.OnHPChange -= OnHPChange;
        }

        public override void _PhysicsProcess(double delta)
        {
            Vector2 velocity = Velocity;

            bool isOnFloor = IsOnFloor();
            bool canMoveFreely = _forcedMovement.IsStopped();
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
                    // Shift down slightly.
                    // For anything except one-way platforms with 1 pixel margin, this will snap the player back to the top
                    Position += Vector2.Down;
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
                KinematicCollision2D collision = GetSlideCollision(i);
                if (collision != null && collision.GetCollider() is IEnemy)
                {
                    TryTakeDamage(collision.GetNormal());
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
            if (_forcedMovement.IsStopped() && PlayerState.Instance.HasUnlock(PlayerState.Progression.BaseAttack))
            {
                // TODO (#31): create "sword" object for collisions
                _sprite.Play(Animation.Attacking);
                _sprite.FrameChanged += CheckAttackFrame;
                SetAnimationTimer(_forcedMovement, Animation.Attacking);
                _forcedMovement.Start();
            }
        }

        public void TryTakeDamage() => TryTakeDamage(Vector2.Zero);
        public void TryTakeDamage(Vector2 fromDirection)
        {
            if (_damageImmunity.IsStopped())
            {
                // TODO: add sound effects
                if (--PlayerState.Instance.HP == 0)
                {
                    Die();
                }
                _sprite.Play(Animation.TakingDamage);
                if (fromDirection != Vector2.Zero)
                {
                    Velocity += 5 * fromDirection.Normalized();
                }
                _forcedMovement.WaitTime = _damageImmunity.WaitTime / 5;
                _damageImmunity.Start();
                _forcedMovement.Start();
            }
        }

        public void Die()
        {
            // TODO: Play death anim or something
            SceneChanger.Instance.GoToGameOver("Your death was in vain.");
        }

        public void DisableCollisions(){            
            _collisionShape.Disabled = true;
        }

        public void EnableCollisions()
        {
            _collisionShape.Disabled = false;
        }
        private void OnHPChange(byte newHp)
        {
            if (newHp < PlayerState.MaxHP)
            {
                if (_optHealTimer != null)
                {
                    // Restart the timer
                    _optHealTimer.Start();
                }
                else
                {
                    _optHealTimer = new()
                    {
                        WaitTime = HealTimeSec
                    };
                    _optHealTimer.Timeout += Heal;
                    AddChild(_optHealTimer);
                    _optHealTimer.Start();
                }
            }
            else
            {
                _optHealTimer?.QueueFree();
                _optHealTimer = null;
            }
        }

        private void Heal()
        {
            if (++PlayerState.Instance.HP > PlayerState.MaxHP)
            {
                GD.PrintErr("Set player HP higher than max HP");
            }
            _healSfx.Play();
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
            timer.WaitTime = _sprite.SpriteFrames.GetFrameCount(animation) / _sprite.SpriteFrames.GetAnimationSpeed(animation);
            timer.OneShot = true;
        }
    }
}
