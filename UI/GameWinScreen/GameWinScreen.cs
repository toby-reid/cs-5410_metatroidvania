using Godot;
using System.Collections.Generic;

namespace UI
{
    public partial class GameWinScreen : Node2D
    {
        [ExportGroup("Set in Object")]
        [Export(PropertyHint.File, "*.txt")] private string _winTextFile;
        [Export] private Label _label;
        [Export] private Timer _timer;
        [Export] private AudioStreamPlayer2D _winSfx;

        // Use this character in the WinScreen.txt to add 1s of delay between portions
        private const char DelayIndicator = '!';

        private List<string> _remainingText;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            string text = ReadTextFile(_winTextFile);
            string[] textParts = text.Split(DelayIndicator);
            _label.Text = textParts[0];
            _remainingText = [.. textParts[1..]];
            _timer.Timeout += AddNextText;
            _timer.OneShot = false;
            _timer.Start();
        }

        private void AddNextText()
        {
            _label.Text += _remainingText[0];
            _remainingText.RemoveAt(0);
            if (_remainingText.Count == 0)
            {
                _timer.Timeout -= AddNextText;
                _winSfx.Play();
                _winSfx.Finished += () => Global.SceneChanger.Instance.ExitGame();
            }
        }

        private static string ReadTextFile(string filePath)
        {
            if (!FileAccess.FileExists(filePath))
            {
                GD.PrintErr("Attempted to open nonexistent file " + filePath);
                return "";
            }
            using var file = FileAccess.Open(filePath, FileAccess.ModeFlags.Read);
            return file.GetAsText();
        }
    }
}
