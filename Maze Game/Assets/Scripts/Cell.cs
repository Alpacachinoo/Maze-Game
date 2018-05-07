using UnityEngine;
using System;

[Serializable]
public class Cell
{
    public int x { get; set; }
    public int y { get; set; }
    public Vector2 worldPosition { get; set; }

    public Cell parent { get; set; }
    public bool visited { get; set; }
    public bool wall = false;
    public bool occupied = false;

    public bool walkable { get; set; }

    public SpriteRenderer tile { get; set; }

    public Block occupant { get; set; }

    public Cell(int x, int y, Vector2 worldPosition, SpriteRenderer tile)
    {
        this.x = x;
        this.y = y;
        this.worldPosition = worldPosition;

        this.tile = tile;
    }
}