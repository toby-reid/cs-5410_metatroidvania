using Godot;
using Global;

public partial class GlitchedDoor : Door
{
    protected override void OnBodyEntered(Node2D body)
    {
        if(body is Actors.Player)
        {
            PlayerState.Instance.CompleteTutorial();
            SceneChanger.Instance.GoToGameOver(
                "COMPUTER OVER. \nVIRUS = VERY YES.\n"
                + "Some functionality corrupted"
            );
        }
    }
}
