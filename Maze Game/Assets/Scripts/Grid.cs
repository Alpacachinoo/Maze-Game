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
    public float CellSize { get { return cellSize;} }

    [SerializeField] private SpriteRenderer tilePrefab;
    [SerializeField] private Transform tilesParent;

    [SerializeField] private Transform selectionTool;

    private Vector2 worldPosition;

    Cell testCell;

    Vector2 mousePos;

    Cell[] testNeighbours;

    Cell test;

    private void Awake()
    {
        instance = this;

        InitializeGrid();
    }

    private void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        testCell = WorldPositionToCell(mousePos);

        testNeighbours = GetNeighbours(testCell, 2);

        test = GetCellBetweenCells(testCell, testNeighbours[1]);

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

                if (x % 2 != 0 || y % 2 != 0)
                    grid[x, y].wall = true;
            }
        }

        StartCoroutine(GenerateMaze());
    }

    private Cell currentCell;
    private Cell previousCell;
    private Cell[] neighbours;

    public int breakPoint;
    private int currentIteration;

    private IEnumerator GenerateMaze()
    {
        currentCell = grid[0, 0];
        currentCell.visited = true;

        while (true)
        {
            neighbours = GetUnvisitedNeighbours(currentCell, 2);

            if (neighbours.Length > 0)
            {
                previousCell = currentCell;
                currentCell = neighbours[Random.Range(0, neighbours.Length)];
                currentCell.parent = previousCell;

                currentCell.visited = true;

                GetCellBetweenCells(currentCell, previousCell).wall = false;
            }
            else
            {
                if (currentCell.parent != null)
                    currentCell = currentCell.parent;
                else
                    break;
            }

            currentIteration++;

            if (currentIteration >= breakPoint)
                break;

            yield return null;
        }
    }

    public Cell WorldPositionToCell(Vector2 worldPos)
    {
        int x = Mathf.Clamp(Mathf.RoundToInt((worldPos.x + gridSize.x / 2) / cellSize), 0, gridWidth - 1);
        int y = Mathf.Clamp(Mathf.RoundToInt((worldPos.y - gridSize.y / 2) / -cellSize), 0, gridHeight - 1);

        return grid[x, y];
    }

    private Cell[] GetNeighbours(Cell cell, int spacing)
    {
        List<Cell> list = new List<Cell>();

        for (int i = -1; i < 1 + 1; i++)
        {
            if (i == 0)
                continue;

            if (i * spacing + cell.x >= 0 && i * spacing + cell.x < gridWidth)
                list.Add(grid[i * spacing + cell.x, cell.y]);

            if (i * spacing + cell.y >= 0 && i * spacing + cell.y < gridHeight)
                list.Add(grid[cell.x, i * spacing + cell.y]);
        }

        return list.ToArray();
    }

    private Cell[] GetUnvisitedNeighbours(Cell cell, int spacing)
    {
        List<Cell> list = new List<Cell>();

        int x;
        int y;

        for (int i = -1; i < 2; i++)
        {
            if (i == 0)
                continue;

            x = i * spacing + cell.x;
            y = i * spacing + cell.y;

            if (x >= 0 && x < gridWidth)
            {
                if (!grid[x, cell.y].visited)
                    list.Add(grid[x, cell.y]);
            }

            if (y >= 0 && y < gridHeight)
            {
                if (!grid[cell.x, y].visited)
                    list.Add(grid[cell.x, y]);
            }
        }

        return list.ToArray();
    }

    private Cell GetCellBetweenCells(Cell a, Cell b)
    {
        Vector2 _a = new Vector2(a.x, a.y);
        Vector2 _b = new Vector2(b.x, b.y);

        Vector2 dir = (_b - _a).normalized;

        return grid[a.x + (int)dir.x, a.y + (int)dir.y];
    }

    private void OnDrawGizmos()
    {
        if (grid != null)
        {
            foreach (Cell cell in grid)
            {
                if (cell.wall)
                {
                    Gizmos.color = Color.red;

                    Gizmos.DrawCube(cell.worldPosition, Vector3.one * cellSize);
                }
            }
        }
    }
}
