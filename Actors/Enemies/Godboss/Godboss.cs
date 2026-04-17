using Godot;
using System;


namespace Actors
{
    public partial class Godboss : CharacterBody2D, IEnemy
    {
        [Export]
        private Vector2 velocity;
        [Export]
        private int HP = 3;
        
        public override void _Ready()
        {
            Velocity = velocity;
        }
        
        public override void _PhysicsProcess(double delta)
        {
            KinematicCollision2D collision = MoveAndCollide(Velocity * (float) delta);
            if (collision != null) {
                Velocity = Velocity.Bounce(collision.GetNormal());
                if (collision.GetCollider() is Player player)
                {
                    player.TryTakeDamage(-collision.GetNormal());
                }
            }
        }
        
        public void TakeDamage()
        {
            HP -= 1;
            if (HP == 0) {
                Die();
            }
            Scale *= 0.8f;
            Velocity *= -1.5f;
        }
        
        public void Die()
        {
            Velocity *= 0f;
            QueueFree();
        }
    }
}
