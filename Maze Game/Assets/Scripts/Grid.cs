using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    private Cell[,] grid;
    public Vector2 gridSize { get { return new Vector2(cellSize * gridWidth, cellSize * gridHeight); } }

    [SerializeField] private int gridWidth;
    [SerializeField] private int gridHeight;

    [SerializeField] private float cellSize;

    [SerializeField] private GameObject tilePrefab;

    private Vector2 worldPosition;

    Cell testCell;

    Vector2 mousePos;

    private void Awake()
    {
        InitializeGrid();
    }

    private void InitializeGrid()
    {
        grid = new Cell[gridWidth, gridHeight];

        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                worldPosition = new Vector2(-gridSize.x / 2 + x * cellSize, gridSize.y / 2 + y * -cellSize);

                grid[x, y] = new Cell(x, y, worldPosition, Instantiate(tilePrefab, worldPosition, Quaternion.identity));

                grid[x, y].tile.transform.localScale = Vector3.one * (cellSize - 0.2f);
            }
        }
    }

    private void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        testCell = WorldPositionToCell(mousePos);
    }

    private Cell WorldPositionToCell(Vector2 worldPos)
    {
        int x = Mathf.Clamp(Mathf.RoundToInt((worldPos.x + gridSize.x / 2) / cellSize), 0, gridWidth - 1);
        int y = Mathf.Clamp(Mathf.RoundToInt((worldPos.y - gridSize.y / 2) / -cellSize), 0, gridHeight - 1);

        return grid[x, y];
    }

    private void OnDrawGizmos()
    {
        if (grid != null)
        {
            foreach (Cell cell in grid)
            {
                Gizmos.color = Color.green;

                if (cell == testCell)
                    Gizmos.color = Color.red;

                Gizmos.DrawCube(cell.worldPosition, Vector2.one * (cellSize - 0.2f));
            }
        }
    }
}
