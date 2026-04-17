using Godot;
using Global;

public partial class MainMenu : Control
{
    [ExportGroup("Buttons")]
    [Export] private Button _newGameButton;
    [Export] private Button _continueButton;
    [Export] private Button _exitButton;

    public override void _Ready()
    {
        _newGameButton.Pressed += NewGame;
        if (!PlayerState.SaveExists())
        {
            _continueButton.Disabled = true;
        }
        else
        {
            _continueButton.Pressed += LoadGame;
        }
        _exitButton.Pressed += () => SceneChanger.Instance.ExitGame();
    }

    private static void NewGame()
    {
        PlayerState.DeleteSaveFile();
        PlayerState.RestoreDefaults();
        SceneChanger.Instance.GoToMainGame();
    }

    private static void LoadGame()
    {
        if (PlayerState.Load())
        {
            SceneChanger.Instance.GoToMainGame();
        }
        else
        {
            SceneChanger.Instance.GoToGameOver("Invalid save file");
        }
    }
}
