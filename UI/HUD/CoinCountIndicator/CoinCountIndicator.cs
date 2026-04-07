using Godot;
using System;

using Global;

namespace UI
{
    public partial class CoinCountIndicator : HBoxContainer
    {
        [Export]
        private Label _label;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            PlayerState.Instance.OnProgression += CheckCoinProgression;
            PlayerState.Instance.OnCoinCountChange += SetCoinIndicator;
            CheckCoinProgression(PlayerState.Instance.Progress);
            SetCoinIndicator(PlayerState.Instance.CoinCount);
        }

        public override void _ExitTree()
        {
            PlayerState.Instance.OnProgression -= CheckCoinProgression;
            PlayerState.Instance.OnCoinCountChange -= SetCoinIndicator;
        }

        private void CheckCoinProgression(PlayerState.Progression progress)
        {
            Visible = progress.HasFlag(PlayerState.Progression.CoinCount);
        }

        private void SetCoinIndicator(ushort coinCount)
        {
            _label.Text = coinCount.ToString();
        }
    }
}
