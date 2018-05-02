using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public int x { get; set; }
    public int y { get; set; }

    public Vector2 worldPosition { get; set; }

    public GameObject tile { get; set; }

    public Cell(int x, int y, Vector2 worldPosition, GameObject tile)
    {
        this.x = x;
        this.y = y;
        this.worldPosition = worldPosition;

        this.tile = tile;
    }
}
