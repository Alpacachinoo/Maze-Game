using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public static Grid instance;

    private Cell[,] grid;
    public Vector2 gridSize { get { return new Vector2(cellSize * gridWidth, cellSize * gridHeight); } }

    [SerializeField] private int gridWidth;
    [SerializeField] private int gridHeight;

    [SerializeField] private float cellSize;
    public float CellSize { get { return cellSize; } }

    [SerializeField] private SpriteRenderer tilePrefab;
    [SerializeField] private Transform tilesParent;

    [SerializeField] private Transform selectionTool;

    private Vector2 worldPosition;

    Cell testCell;

    Vector2 mousePos;

    Cell[] testNeighbours;

    private void Awake()
    {
        instance = this;

        InitializeGrid();
    }

    private void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        testCell = WorldPositionToCell(mousePos);

        testNeighbours = GetNeighbours(testCell);

        selectionTool.position = testCell.worldPosition;
    }

    private void InitializeGrid()
    {
        grid = new Cell[gridWidth, gridHeight];
        selectionTool.transform.localScale = Utility.DivideFloatByVector(cellSize, tilePrefab.sprite.bounds.size);

        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                worldPosition = new Vector2(-gridSize.x / 2 + x * cellSize, gridSize.y / 2 + y * -cellSize);

                grid[x, y] = new Cell(x, y, worldPosition, Instantiate(tilePrefab.gameObject, worldPosition, Quaternion.identity, tilesParent).GetComponent<SpriteRenderer>());

                grid[x, y].tile.transform.localScale = new Vector3(cellSize / grid[x, y].tile.sprite.bounds.size.x, cellSize / grid[x, y].tile.sprite.bounds.size.y, 1);
            }
        }
    }

    public Cell WorldPositionToCell(Vector2 worldPos)
    {
        int x = Mathf.Clamp(Mathf.RoundToInt((worldPos.x + gridSize.x / 2) / cellSize), 0, gridWidth - 1);
        int y = Mathf.Clamp(Mathf.RoundToInt((worldPos.y - gridSize.y / 2) / -cellSize), 0, gridHeight - 1);

        return grid[x, y];
    }

    public Cell[] GetNeighbours(Cell cell)
    {
        List<Cell> list = new List<Cell>();

        for (int i = -1; i < 2; i++)
        {
            if (i == 0)
                continue;

            if (i + cell.x > 0 && i + cell.x < gridWidth)
                list.Add(grid[i + cell.x, cell.y]);

            if (i + cell.y > 0 && i + cell.y < gridHeight)
                list.Add(grid[cell.x, i + cell.y]);
        }

        return list.ToArray();
    }

    private void OnDrawGizmos()
    {
        if (testNeighbours != null)
        {
            foreach (Cell cell in testNeighbours)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawCube(cell.worldPosition, Vector3.one * cellSize);
            }
        }
    }
}
