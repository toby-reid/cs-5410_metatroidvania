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
        // placeholder until continue is implemented
        _continueButton.Pressed += () => SceneChanger.Instance.GoToGameOver();
        _exitButton.Pressed += () => SceneChanger.Instance.ExitGame();
    }
}
