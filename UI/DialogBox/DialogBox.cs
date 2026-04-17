using Godot;

public partial class DialogBox : CanvasLayer
{
    [Export] Label title;
    [Export] Label instructions;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Global.ItemBus.Instance.ItemCollected += Display;
    }

    public override void _Input(InputEvent @event)
    {
        if (Visible && @event.IsActionPressed(Global.Constants.InputMap.Jump))
        {
            Global.PauseManager.Instance.ResumeGame();
            Visible = false;
        }
    }

    public override void _ExitTree()
    {
        Global.ItemBus.Instance.ItemCollected -= Display;
    }

    private void Display(Item item)
    {
        title.Text = $"You Fixed '{item.ItemName}'!";
        instructions.Text = item.Instructions;
        Visible = true;
        Global.PauseManager.Instance.PauseGame();
    }
}
