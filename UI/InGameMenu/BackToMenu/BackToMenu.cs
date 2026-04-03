using Godot;
using System;

namespace UI
{
    public partial class BackToMenu : Control
    {
        [Export]
        private BaseButton _quitButton;
        [Export]
        private Control _confirmationDialogue;
        [Export]
        private BaseButton _confirmButton;
        [Export]
        private BaseButton _cancelButton;

        [Export(PropertyHint.File)]
        private string _mainMenuPath;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            _quitButton.Pressed += ShowConfirmation;
            _confirmButton.Pressed += ToMenu;
            _cancelButton.Pressed += HideConfirmation;
        }

        public override void _Input(InputEvent @event)
        {
            if (@event.IsActionPressed(Global.Constants.InputMap.Pause))
            {
                HideConfirmation();
            }
        }

        private void ShowConfirmation()
        {
            _quitButton.Visible = false;
            _confirmationDialogue.Visible = true;
        }

        private void HideConfirmation()
        {
            _quitButton.Visible = true;
            _confirmationDialogue.Visible = false;
        }

        private void ToMenu()
        {
            Global.SceneChanger.Instance.ChangeScene(_mainMenuPath);
        }
    }
}
