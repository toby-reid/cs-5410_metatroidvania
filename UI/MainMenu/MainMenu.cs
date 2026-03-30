using Godot;
using Global;

public partial class MainMenu : Control
{
    [ExportGroup("Buttons")]
    [Export] private Button _newGameButton;
    [Export] private Button _continueButton;
    [Export] private Button _exitButton;

    [ExportGroup("Scenes")]
    [Export(PropertyHint.File, "*.tscn")] private string _mainGamePath;

    public override void _Ready()
    {
        _newGameButton.Pressed += () => SceneChanger.Instance.ChangeScene(_mainGamePath);
        _continueButton.Pressed += () => GD.Print("TODO: connect to continue game");
        _exitButton.Pressed += () => SceneChanger.Instance.ExitGame();
    }
}
