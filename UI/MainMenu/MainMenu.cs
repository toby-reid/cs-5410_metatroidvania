using Godot;
using Global;

public partial class MainMenu : Control
{
    [Export] private Button _newGameButton;
    [Export] private Button _continueButton;
    [Export] private Button _exitButton;
    
    public override void _Ready()
    {
        _newGameButton.Pressed += () => GD.Print("TODO: connect to new game");
        _continueButton.Pressed += () => GD.Print("TODO: connect to continue game");
        _exitButton.Pressed += () => SceneChanger.Instance.ExitGame();
    }
}
