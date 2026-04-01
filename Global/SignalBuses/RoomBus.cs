using Godot;

namespace Global
{
    public partial class RoomBus : Node
    {
        public static RoomBus Instance { get; private set; }

        [Signal]
        public delegate void RoomChangeRequestEventHandler(string scenePath, int doorId);

        public override void _Ready()
        {
            Instance = this;
        }

        public void EmitRoomChange(string scenePath, int doorId)
        {
            EmitSignal(SignalName.RoomChangeRequest, scenePath, doorId);
        }
    }
}
