using Godot;
using System;

/// signal bus for managing room change signals
[GlobalClass]
public partial class RoomChangeBus : Resource
{
    public event Action<string, int> OnRoomChangeRequest;

    public void Raise(string scenePath, int doorId)
    {
        OnRoomChangeRequest?.Invoke(scenePath, doorId);
    }
}
