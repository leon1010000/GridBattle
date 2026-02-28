using UnityEngine;
using System.Collections.Generic;
public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }
    public int width = 5;
    public int height = 5;
    public GridCell cellPrefab;
    public float cellSize = 1f;
    public Vector3 origin = new(0, 0, 0);
    private GridData[,] gridData;
    private GridCell[,] gridCells;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        gridData = new GridData[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                gridData[i, j] = new GridData();
            }
        }
        origin = new(-width * cellSize / 2, -height * cellSize / 2);
    }
    void Start()
    {
        DrawGrid();
    }
    public void RegisterUnit(Unit unit)
    {
        Vector2Int pos = unit.gridPosition;
        if (!IsInsideGrid(pos)) return;
        gridData[pos.x, pos.y].unit = unit;
    }
    public void UnRegisterUnit(Unit unit)
    {
        Vector2Int pos = unit.gridPosition;
        if (!IsInsideGrid(pos)) return;
        gridData[pos.x, pos.y].unit = null;
    }
    public bool MoveUnit(Unit unit, Vector2Int newPos)
    {
        Vector2Int pos = unit.gridPosition;
        if (!CanMove(newPos) || !IsInsideGrid(pos)) return false;
        gridData[pos.x, pos.y].unit = null;
        gridData[newPos.x, newPos.y].unit = unit;
        return true;
    }
    public Unit GetUnit(Vector2Int pos)
    {
        return gridData[pos.x, pos.y].unit;
    }
    public Vector3 GridToWorld(Vector2Int gridPos)
    {
        return new Vector3(gridPos.x * cellSize + origin.x + cellSize / 2, gridPos.y * cellSize + origin.y + cellSize / 2, 0);
    }
    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        Vector2Int gridPos = new(
            Mathf.FloorToInt((worldPos.x - origin.x) / cellSize),
            Mathf.FloorToInt((worldPos.y - origin.y) / cellSize)
            );
        if (IsInsideGrid(gridPos)) return gridPos;
        return new(-1, -1);
    }
    public bool IsInsideGrid(Vector2Int pos)
    {
        return 0 <= pos.x && pos.x < width && 0 <= pos.y && pos.y < height;
    }
    public bool CanMove(Vector2Int pos)
    {
        return IsInsideGrid(pos) && CanMoveGridData(gridData[pos.x, pos.y]);
    }
    bool CanMoveGridData(GridData data)
    {
        return data.unit == null;
    }
    public List<Unit> GetTargets(Unit unit, Vector2Int[] damagePos)
    {
        List<Unit> units = new();
        foreach (Vector2Int relativePos in damagePos)
        {
            Vector2Int pos = unit.gridPosition + relativePos;
            if (IsInsideGrid(pos) && gridData[pos.x, pos.y].unit != null)
            {
                units.Add(gridData[pos.x, pos.y].unit);
            }
        }
        return units;
    }
    void DrawGrid()
    {
        gridCells = new GridCell[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                CreateCell(new Vector2Int(i, j));
            }
        }
    }
    void CreateCell(Vector2Int pos)
    {
        if (!IsInsideGrid(pos)) return;
        GridCell cell = Instantiate(cellPrefab, GridToWorld(pos), Quaternion.identity, transform);
        cell.transform.localScale = new Vector3(cellSize, cellSize, 1);
        cell.gridPos = pos;
        cell.SetColor(GridColor.Default);
        gridCells[pos.x, pos.y] = cell;
    }
    public void SetGridColor(List<Vector2Int> positions, Color color)
    {
        foreach (Vector2Int pos in positions)
        {
            if (!IsInsideGrid(pos)) continue;
            gridCells[pos.x, pos.y].SetColor(color);
        }
    }
    public void ClearGridColor()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                gridCells[i, j].SetColor(GridColor.Default);
            }
        }
    }
}
