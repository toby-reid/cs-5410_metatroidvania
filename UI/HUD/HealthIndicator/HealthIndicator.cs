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
            SetMaxHP(PlayerState.MaxHP);
        }

        public override void _ExitTree()
        {
            PlayerState.Instance.OnProgression -= CheckHPProgression;
            PlayerState.Instance.OnHPChange -= SetHealth;
        }

        private void SetMaxHP(byte maxHp)
        {
            byte currentHearts = (byte)_heartsContainer.GetChildCount();
            if (currentHearts > maxHp)
            {
                var hearts = _heartsContainer.GetChildren();
                for (byte i = maxHp; i < currentHearts; ++i)
                {
                    hearts[i].QueueFree();
                }
            }
            else
            {
                for (int i = currentHearts; i < maxHp; ++i)
                {
                    AddHeart();
                }
            }
            SetHealth(PlayerState.Instance.HP);
        }

        private void CheckHPProgression(PlayerState.Progression progress)
        {
            Visible = progress.HasFlag(PlayerState.Progression.HealthBar);
        }

        private void SetHealth(byte newHp)
        {
            var hearts = _heartsContainer.GetChildren();
            if (hearts.Count != PlayerState.MaxHP || newHp > hearts.Count)
            {
                GD.PrintErr($"Mismatch in hearts/HP: {newHp}/{PlayerState.MaxHP} ({hearts.Count})");
                return;
            }
            for (byte i = 0; i < PlayerState.MaxHP; ++i)
            {
                if (hearts[i] is Heart heart)
                {
                    heart.IsActive = i < newHp;
                }
                else
                {
                    GD.PrintErr("Found unrecognized hearts container child " + hearts[i]);
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
