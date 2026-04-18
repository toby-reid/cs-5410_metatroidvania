using Godot;
using Global;
using Actors;

public partial class RoomManager : Node
{
    [Export] public Node2D _roomContainer;
    [Export] public Camera2D _camera;
    [Export] private Player _player;

    public override void _Ready()
    {
        ChangeRoom(PlayerState.Instance.CurrentRoom, PlayerState.Instance.LastUsedDoorwayID);
        RoomBus.Instance.RoomChangeRequest += ChangeRoom;
    }

    private async void ChangeRoom(string scene, string doorID)
    {

        _player.DisableCollisions();
        // there should only be one child, but this is safer
        foreach (Node room in _roomContainer.GetChildren())
        {
            room.QueueFree();
        }

        BaseRoom newRoom = (BaseRoom)ResourceLoader.Load<PackedScene>(scene).Instantiate();
        UpdateCamera(newRoom);

        _roomContainer.AddChild(newRoom);
        _player.Position = newRoom.GetSpawnPosition(doorID);

        PlayerState.Instance.CurrentRoom = scene;
        PlayerState.Instance.LastUsedDoorwayID = doorID;

        await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
        _player.EnableCollisions();
    }

    public override void _ExitTree()
    {
        RoomBus.Instance.RoomChangeRequest -= ChangeRoom;
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
