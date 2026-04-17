using Godot;
using System;
using Global;

public partial class Item : Area2D
{
    [Export] public string ItemName;
    [Export(PropertyHint.MultilineText)] public string Instructions;
    [Export] private PlayerState.Progression _itemValue;

    [Export] private Area2D _area;

    public override void _Ready()
    {
        _area.BodyEntered += OnAreaEntered;
    }

    private void OnAreaEntered(Node2D area)
    {
        if (area is Actors.Player)
        {
            PlayerState.Instance.Unlock(_itemValue);
            ItemBus.Instance.EmitItemCollected(this);
            QueueFree();
        }
    }
}
