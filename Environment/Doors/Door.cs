using Godot;
using Global;

public partial class Door : Area2D
{
    [ExportGroup("Internal Values")]
    [Export] public string ID;
    [Export] private Marker2D _spawnPoint;

    [ExportGroup("Target Values")]
    [Export(PropertyHint.File, "*.tscn")]
    private string _targetRoomPath;
    [Export] private string _targetDoorID;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        
    }

    public Vector2 GetSpawnPosition(){
        return _spawnPoint.GlobalPosition;
    }

    private void OnBodyEntered(Node2D body)
    {
        if(body is Actors.Player)
        {
            // Global.RoomBus.Instance.EmitRoomChange(_targetRoomPath, _targetDoorID);
            // Inside Door.cs OnBodyEntered
            RoomBus.Instance.CallDeferred(nameof(RoomBus.EmitRoomChange), _targetRoomPath, _targetDoorID);
        }
    }
}
