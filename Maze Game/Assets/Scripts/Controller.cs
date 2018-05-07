using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public static Controller instance;

    private Action currentAction;
    private bool inUse = false;

    private Cell currentCell;
    private Block selectedBlock;
    private Cell[] availableCells;
    [SerializeField] private Transform selectionMarker;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        UpdateHighlightedCell();

        if (Input.GetMouseButtonDown(0))
        {
            if (!inUse)
            {
                if (currentCell.occupant != null)
                    currentCell.occupant.Select();
            }
            else
            {
                if (Grid.instance.WithinRadius(selectedBlock._occupiedCell, currentCell, 1))
                {
                    switch (currentAction)
                    {
                        case Action.MOVE:
                            MoveBlock(selectedBlock, currentCell);
                            break;
                        default:
                            break;
                    }
                }
                ResetAction();
            }
        }
    }

    public void AwaitAction(Block block)
    {
        if (currentAction == Action.NONE)
        {
            selectedBlock = block;
            currentAction = selectedBlock.type;
            availableCells = selectedBlock.neighbours;
            inUse = true;
        }
    }

    private void ResetAction()
    {
        currentAction = Action.NONE;
        selectedBlock = null;
        availableCells = null;
        inUse = false;
    }

    private void UpdateHighlightedCell()
    {
        currentCell = Grid.instance.WorldPositionToCell(Utility.MousePos);
        selectionMarker.position = currentCell.worldPosition;
    }

    private void MoveBlock(Block block, Cell targetCell)
    {
        block.OccupyCell(targetCell);
    }

    private void OnDrawGizmos()
    {
        if (availableCells != null)
        {
            foreach (Cell cell in availableCells)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(cell.worldPosition, 0.2f);
            }
        }
    }
}