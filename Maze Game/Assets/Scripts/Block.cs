using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public Action type { get { return _type; } }
    [SerializeField] private Action _type = Action.NONE;

    private Cell occupiedCell;
    public Cell _occupiedCell { get { return occupiedCell; } }

    public Cell[] neighbours { get { return _neighbours; } }
    private Cell[] _neighbours;

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

    public void OccupyCell(Cell cell)
    {
        RemoveFromCurrentCell();

        occupiedCell = cell;
        occupiedCell.occupant = this;
        occupiedCell.occupied = true;
        occupiedCell.walkable = false;

        transform.position = occupiedCell.worldPosition;
    }

    private void RemoveFromCurrentCell()
    {
        if (occupiedCell != null)
        {
            occupiedCell.occupant = null;
            occupiedCell.walkable = true;
            occupiedCell.occupied = false;
            occupiedCell = null;
        }
    }

    public void Select()
    {
        if (type != Action.NONE)
        {
            _neighbours = Grid.instance.GetNeighbours(occupiedCell, 1);

            Controller.instance.AwaitAction(this);
        }
    }
}

public enum Action { NONE, MOVE}
