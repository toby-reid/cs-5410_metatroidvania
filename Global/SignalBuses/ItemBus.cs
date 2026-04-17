using Godot;

namespace Global
{
    public partial class ItemBus : Node
    {
        public static ItemBus Instance { get; private set; }

        [Signal]
        public delegate void ItemCollectedEventHandler(Item item);

        public override void _Ready()
        {
            Instance = this;
        }

        public void EmitItemCollected(Item item)
        {
            EmitSignal(SignalName.ItemCollected, item);
        }
    }
    
}
