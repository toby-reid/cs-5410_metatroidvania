using Godot;

public partial class RoomManager : Node
{
    [Export] public Node2D _roomContainer;
    [Export] public RoomChangeBus _roomChangeBus;
    
    [ExportGroup("Start Room")]
    [Export(PropertyHint.File, "*.tscn")] private string startRoom;
    [Export] private int startDoor;

    public override void _Ready()
    {
        ChangeRoom(startRoom, startDoor);
        _roomChangeBus.RoomChangeRequest += ChangeRoom;
    }

    private void ChangeRoom(string scene, int doorId)
    {
        // there should only be one child, but this is safer
        foreach (Node room in _roomContainer.GetChildren())
        {
            room.QueueFree();
        }

        Node newRoom = ResourceLoader.Load<PackedScene>(scene).Instantiate();
        _roomContainer.AddChild(newRoom);

        // TODO put the player at the room
    }
}
