using Godot;
using System;


namespace Actors
{
    public partial class Bat : CharacterBody2D, IEnemy
    {
        private static class Animation
        {
            public const string Idle = "idle";
            public const string TakingDamage = "take_damage";
            public const string AttackBegin = "attack_begin";
            public const string Attack = "attack";
            public const string AttackEnd = "attack_end";
        }

        private enum State
        {
            Attacking,
            Pursuing,
            Idle,
            Dying
        }

        [Export]
        private AnimatedSprite2D _sprite;
        [Export]
        private Node2D _idleTarget;
        [Export]
        private Area2D _playerDetector;
        [Export]
        private CollisionShape2D _collision;

        private State state = State.Idle;
        private Player player;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            _sprite.Play(Animation.Idle);
            _sprite.AnimationFinished += AdvanceAnimation;
        }

        public void AdvanceAnimation()
        {
            if (_sprite.Animation == Animation.AttackBegin) {
                Velocity = (player.GlobalPosition - GlobalPosition).Normalized() * 100;
                _sprite.Play(Animation.Attack);
            } else if (_sprite.Animation == Animation.Attack) {
                _sprite.Play(Animation.AttackEnd);
            } else if (_sprite.Animation == Animation.AttackEnd) {
                _sprite.Play(Animation.Idle);
                state = State.Pursuing;
                if (_playerDetector.GetOverlappingBodies().Contains(player)) {
                    OnBodyDetected(player);
                }
            } else if (_sprite.Animation == Animation.TakingDamage) {
                QueueFree();
            }
        }

        public void Die()
        {
            Velocity = Vector2.Zero;
            state = State.Dying;
            _sprite.Play(Animation.TakingDamage);
            _collision.QueueFree();
            _collision = null;
        }

        public override void _PhysicsProcess(double delta)
        {
            if (state == State.Pursuing) {
                Vector2 target = player.GlobalPosition - GlobalPosition;
                target.Y -= 20;
                Velocity = target.Normalized() * 65;
            }
            if (state == State.Idle) return;
            if (Velocity.X > 0) _sprite.FlipH = true;
            else if (Velocity.X < 0) _sprite.FlipH = false;
            KinematicCollision2D collision = MoveAndCollide(Velocity * (float) delta);
            if (collision != null)
            {
                Velocity = Velocity.Bounce(collision.GetNormal()) * 0.6f;
                if (collision.GetCollider() is Player player)
                {
                    player.TakeDamage();
                }
            }
        }

        public override void _Process(double delta)
        {
            if (state != State.Idle) return;
            _sprite.FlipH = Position.X < _idleTarget.Position.X;
            Position = _idleTarget.Position;
        }

        public void OnBodyDetected(Node2D body) {
            if (body is Player && state != State.Attacking && state != State.Dying) {
                player = (Player) body;
                state = State.Attacking;
                _sprite.Play(Animation.AttackBegin);
                Velocity *= 0f;
            }
        }
    }
}
