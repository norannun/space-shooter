using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell
{
    public int IndexX { get; private set; }
    public int IndexY { get; private set; }
    public Vector2 Position { get; private set; }
    public bool IsOccupied { get; set; }
    public CellCluster Cluster { get; private set; }

    public GridCell(int xIndex, int yIndex, Vector2 initialPoint, float cellSize)
    {
        IndexX = xIndex;
        IndexY = yIndex;
        Position = new Vector2(
            initialPoint.x + cellSize * xIndex,
            initialPoint.y + cellSize * yIndex
        );
        IsOccupied = false;
    }


    public void Occupy(CellCluster cluster)
    {
        IsOccupied = true;
        Cluster = cluster;
    }

    public void Free()
    {
        IsOccupied = false;
        Cluster = null;
    }
}


public class CellCluster
{
    public GridCell Cell { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }

    private float cellSize;

    public CellCluster(GridCell cell, Vector2 objSize, float cellSize)
    {
        Cell = cell;
        this.cellSize = cellSize;
        Width = (int)Mathf.Round(objSize.x / (2 * cellSize));
        Height = (int)Mathf.Round(objSize.y / (2 * cellSize));
    }

    public void SetCell(GridCell cell)
    {
        Cell = cell;
    }

    public void OccupyEntire(GridCell[,] grid)
    {
        for (int y = Cell.IndexY - Height; y <= Cell.IndexY + Height; y++)
        {
            for (int x = Cell.IndexX - Width; x <= Cell.IndexX + Width; x++)
            {
                if (IsValidCell(x, y, grid))
                {
                    grid[y, x].Occupy(this);
                }
            }
        }
    }

    public void OccupyRow(GridCell[,] grid, int row)
    {
        for (int x = Cell.IndexX - Width; x <= Cell.IndexX + Width; x++)
        {
            if (IsValidCell(x, row, grid))
            {
                grid[row, x].Occupy(this);
            }
        }
    }

    public void FreeEntire(GridCell[,] grid)
    {
        for (int y = Cell.IndexY - Height; y <= Cell.IndexY + Height; y++)
        {
            for (int x = Cell.IndexX - Width; x <= Cell.IndexX + Width; x++)
            {
                if (IsValidCell(x, y, grid))
                {
                    grid[y, x].Free();
                }
            }
        }
    }

    public void FreeRow(GridCell[,] grid, int row)
    {
        for (int x = Cell.IndexX - Width; x <= Cell.IndexX + Width; x++)
        {
            if (IsValidCell(x, row, grid))
            {
                grid[row, x].Free();
            }
        }
    }

    private bool IsValidCell(int x, int y, GridCell[,] grid)
    {
        return x >= 0 && x < grid.GetLength(1) && y >= 0 && y < grid.GetLength(0);
    }
}


public class SpawnPositionManager_ : MonoBehaviour
{
    public static SpawnPositionManager_ Instance { get; private set; }
    [SerializeField] private SpawnPositionManagerConfig config;

    private int gridWidth;
    private int gridHeight;

    private Vector2 initialPoint;
    // public Vector2 InitialPoint => initialPoint;

    public float CellSize { get; private set; }
    private int currentGridHeight;
    private GridCell[,] grid;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializeGrid();
    }

    private void InitializeGrid()
    {
        gridWidth = config.gridInitialSizeX;
        gridHeight = config.gridInitialSizeY;

        float screenWidth = GlobalValuesManager.Instance.RightBoundary - GlobalValuesManager.Instance.LeftBoundary;
        CellSize = screenWidth / gridWidth;

        initialPoint = new Vector2(
            GlobalValuesManager.Instance.LeftBoundary + CellSize / 2,
            GlobalValuesManager.Instance.TopBoundary + CellSize / 2
        );

        currentGridHeight = gridHeight;
        grid = new GridCell[currentGridHeight, gridWidth];

        for (int y = 0; y < currentGridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                grid[y, x] = new GridCell(x, y, initialPoint, CellSize);
            }
        }
    }


    public GridCell GetCell(int xIndex, int yIndex)
    {
        if (IsValidCell(xIndex, yIndex))
        {
            return grid[yIndex, xIndex];
        }
        return null;
    }

    public Vector2 GetRandomPosition<T>(T obj) where T : SpawnableObject
    {
        CellCluster cluster = FindAvailableCluster(obj.Size);
        if (cluster != null)
        {
            cluster.OccupyEntire(grid);
            StartCoroutine(OccupationSequnce(cluster, obj.SpeedY));
            return cluster.Cell.Position;
        }
        else
        {
            Debug.LogWarning("No available position found for spawning object.");
            return Vector2.zero;
        }
    }

    private CellCluster FindAvailableCluster(Vector2 objSize)
    {
        int clusterWidth = (int)Mathf.Round(objSize.x / (2 * CellSize));
        int clusterHeight = (int)Mathf.Round(objSize.y / (2 * CellSize));

        List<Vector2Int> positions = new List<Vector2Int>();
        for (int y = 0; y < currentGridHeight; y++)  // O(n^2), n - grid size
        {
            for (int x = 0; x < gridWidth; x++)
            {
                positions.Add(new Vector2Int(x, y));
            }
        }
        Shuffle(positions);  // O(n), n - grid size

        foreach (var pos in positions)  // O(n), n - grid size
        {
            GridCell cell = grid[pos.y, pos.x];
            if (IsSpaceAvailable(cell, clusterWidth, clusterHeight))
            {
                return new CellCluster(cell, objSize, CellSize);
            }
        }

        // Problem
        ExpandGrid(clusterHeight);
        return FindAvailableCluster(objSize);
    }

    private IEnumerator OccupationSequnce(CellCluster cluster, float speed)
    {
        float delay = CellSize / speed;
        int pointerY = cluster.Height;  // Only when cluster.IndexY == 0

        bool notDone = true;
        while (notDone)
        {
            yield return new WaitForSeconds(delay);

            if (IsValidCell(cluster.Cell.IndexX, cluster.Cell.IndexY - 1))
            {
                cluster.OccupyRow(grid, cluster.Cell.IndexY - cluster.Height - 1);
                cluster.FreeRow(grid, cluster.Cell.IndexY + cluster.Height);
                cluster.SetCell(GetCell(cluster.Cell.IndexX, cluster.Cell.IndexY - 1));
            }
            else if (IsValidCell(cluster.Cell.IndexX, pointerY))
            {
                cluster.FreeRow(grid, pointerY);
                pointerY--;
            }
            else
            {
                notDone = false;
            }
        }
    }

    private void Shuffle(List<Vector2Int> list)  // O(n), n - grid size
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);
            Vector2Int temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    private bool IsSpaceAvailable(GridCell cell, int clusterWidth, int clusterHeight)
    {
        for (int y = cell.IndexY - clusterHeight; y <= cell.IndexY + clusterHeight; y++)
        {
            for (int x = cell.IndexX - clusterWidth; x <= cell.IndexX + clusterWidth; x++)
            {
                if (!IsValidCell(x, y) || grid[y, x].IsOccupied)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private bool IsValidCell(int x, int y)
    {
        return x >= 0 && x < gridWidth && y >= 0 && y < currentGridHeight;
    }

    private void ExpandGrid(int additionalHeight)
    {
        int newHeight = currentGridHeight + additionalHeight;
        GridCell[,] newGrid = new GridCell[newHeight, gridWidth];

        for (int y = 0; y < currentGridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                newGrid[y, x] = grid[y, x];
            }
        }

        for (int y = currentGridHeight; y < newHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                newGrid[y, x] = new GridCell(x, y, initialPoint, CellSize);
            }
        }

        grid = newGrid;
        currentGridHeight = newHeight;
    }

    private void ContractGrid(int heightToReduce)
    {
        int newHeight = currentGridHeight - heightToReduce;
        GridCell[,] newGrid = new GridCell[newHeight, gridWidth];

        for (int y = 0; y < newHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                newGrid[y, x] = grid[y, x];
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (grid == null) return;

        for (int y = 0; y < currentGridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                GridCell cell = grid[y, x];
                Vector3 position = new Vector3(cell.Position.x, cell.Position.y, 0);
                Gizmos.color = cell.IsOccupied ? Color.red : Color.green;
                Gizmos.DrawWireCube(position, Vector3.one * CellSize);
            }
        }
    }
}


[CreateAssetMenu(fileName = "SpawnPositionManager Config", menuName = "Configs/Spawn System/SpawnPositionManager Config")]
public class SpawnPositionManagerConfig : ScriptableObject
{
    public int gridInitialSizeX;
    public int gridInitialSizeY;
}