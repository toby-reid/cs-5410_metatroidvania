using Godot;
using System;

public partial class BaseRoom : Node2D
{
    [Export] private TileMapLayer _tileMapLayer;

    public Rect2 GetRoomLimits()
    {
        Rect2I usedRect = _tileMapLayer.GetUsedRect();
        int cellSize = _tileMapLayer.TileSet.TileSize.X;

        return new Rect2(
            usedRect.Position.X * cellSize,
            usedRect.Position.Y * cellSize,
            usedRect.Size.X * cellSize,
            usedRect.Size.Y * cellSize
        );
    }
}
