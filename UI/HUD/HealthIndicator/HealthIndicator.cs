using Godot;
using System;

using Global;

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
            PlayerState.Instance.OnProgression += CheckHPProgression;
            PlayerState.Instance.OnHPChange += SetHealth;
            CheckHPProgression(PlayerState.Instance.Progress);
            SetHealth(PlayerState.Instance.HP);
        }

        private void CheckHPProgression(PlayerState.Progression progress)
        {
            Visible = progress.HasFlag(PlayerState.Progression.HealthBar);
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
