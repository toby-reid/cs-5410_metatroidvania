using Godot;
using Global;

public partial class RoomManager : Node
{
    [Export] public Node2D _roomContainer;
    [Export] public Camera2D _camera;

    [ExportGroup("Start Room")]
    [Export(PropertyHint.File, "*.tscn")] private string startRoom;
    [Export] private int startDoor;

    public override void _Ready()
    {
        ChangeRoom(startRoom, startDoor);
        RoomBus.Instance.RoomChangeRequest += ChangeRoom;
    }

    private void ChangeRoom(string scene, int doorId)
    {
        // there should only be one child, but this is safer
        foreach (Node room in _roomContainer.GetChildren())
        {
            room.QueueFree();
        }

        Node newRoom = ResourceLoader.Load<PackedScene>(scene).Instantiate();
        UpdateCamera((BaseRoom)newRoom);
        _roomContainer.AddChild(newRoom);

        // TODO put the player at the room
    }

    private void UpdateCamera(BaseRoom room)
    {
        Rect2 limits = room.GetRoomLimits();

        _camera.LimitLeft = (int)limits.Position.X;
        _camera.LimitTop = (int)limits.Position.Y;
        _camera.LimitRight = (int)limits.End.X;
        _camera.LimitBottom = (int)limits.End.Y;

        // weird name, this is a sharp, not smooth, transition
        _camera.ResetSmoothing();
    }
}
