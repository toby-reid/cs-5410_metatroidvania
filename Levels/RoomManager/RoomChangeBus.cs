using Godot;

/// signal bus for managing room change signals
[GlobalClass]
public partial class RoomChangeBus : Resource
{

    [Signal]
    public delegate void RoomChangeRequestEventHandler(string scenePath, int doorId);

    public void Raise(string scenePath, int doorId)
    {
        EmitSignal(SignalName.RoomChangeRequest, scenePath, doorId);
    }
}
