using Godot;
using System;

namespace UI
{
    public partial class HealthIndicator : HBoxContainer
    {
        [Export]
        private PackedScene _heart;
        [Export]
        private Container _heartsContainer;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
        }

        private void SetHealth(byte newHp)
        {
            byte currentHearts = (byte)_heartsContainer.GetChildCount();
            if (currentHearts > newHp)
            {
                var hearts = _heartsContainer.GetChildren();
                for (int i = 0; i < currentHearts - newHp; ++i)
                {
                    hearts[i].QueueFree();
                }
            }
            else
            {
                for (int i = currentHearts; i < newHp; ++i)
                {
                    AddHeart();
                }
            }
        }

        private void AddHeart()
        {
            Node heart = _heart.Instantiate();
            _heartsContainer.AddChild(heart);
        }
    }
}
