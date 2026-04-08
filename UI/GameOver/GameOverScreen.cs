using Godot;
using System;
using Global;

public partial class GameOverScreen : Control
{
    [Export] Label restartLabel;
    [Export] RichTextLabel errorLabel;
    
    private double countdown = 5;
    private String errorMessage =
        "[bgcolor=white][color=blue] Error:[/color][/bgcolor] COMPUTER OVER. \nVIRUS = VERY YES.";
    

    public void Init(string error)
    {
        if (error != null)
        {
            errorMessage = "Error:[/color][/bgcolor] " + error;
        }
    }
    
    public override void _Ready()
    {
        errorLabel.Text = errorMessage;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (countdown >= 0)
        {
            countdown -= delta;
            restartLabel.Text = "[r]estart: " + (int)Math.Round(countdown);
        }
        else
        {
            // TODO call continue game
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey keyEvent && keyEvent.Pressed)
        {
            if (keyEvent.Keycode == Key.R)
            {
                // TODO call continue game
            }
            else if (keyEvent.Keycode == Key.E)
            {
                SceneChanger.Instance.ExitGame();
            }
        }
}
}
