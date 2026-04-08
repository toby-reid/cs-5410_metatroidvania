using Godot;
using System;
using Global;

public partial class Item : Area2D
{
    [Export] private String _name;
    [Export(PropertyHint.MultilineText)] private String _instructions;
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
            QueueFree();
        }

        // TODO show dialog box and pause game
    }
}
