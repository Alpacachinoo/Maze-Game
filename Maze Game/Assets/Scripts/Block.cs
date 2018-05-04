using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private Cell occupiedCell;
    public Cell _occupiedCell { get { return occupiedCell; } }

    private SpriteRenderer sr;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        sr = GetComponent<SpriteRenderer>();

        //transform.localScale = Utility.DivideFloatByVector(Grid.instance.CellSize, sr.sprite.bounds.size);

        OccupyCell(Grid.instance.WorldPositionToCell(transform.position));
    }

    public void Move(Cell cell)
    {
        RemoveFromCurrentCell();

        OccupyCell(cell);
    }

    private void OccupyCell(Cell cell)
    {
        occupiedCell = cell;
        occupiedCell.occupant = this;
        occupiedCell.walkable = false;

        transform.position = occupiedCell.worldPosition;
    }

    private void RemoveFromCurrentCell()
    {
        occupiedCell.occupant = null;
        occupiedCell.walkable = true;
        occupiedCell = null;
    }
}
