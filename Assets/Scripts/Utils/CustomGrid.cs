///
/// Author: Marvin Kalchschmidt : mk306
/// Description: Grid Script, Helper Script for a 2D Grid of any datatype (from my Curse of Elements Project)
/// ==============================================
/// Changelog: 
/// ==============================================
///

using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomGrid<T>
{
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private float _cellWidth;
    [SerializeField] private float _cellHeight;
    [SerializeField] private Vector3 _originPosition;
   
    private T[,] grid;

    //Constructor if cells are Squares
    public CustomGrid(int width, int height, float cellSize, Vector3 originPosition)
    {
        this._width = width;
        this._height = height;
        this._cellWidth = cellSize;
        this._cellHeight= cellSize;
        this._originPosition = originPosition;

        grid = new T[width, height];
        //DebugTest();
    }

    public CustomGrid(int width, int height, float cellWidth, float cellHeight, Vector3 originPosition)
    {
        this._width = width;
        this._height = height;
        this._cellWidth = cellWidth;
        this._cellHeight = cellHeight;
        this._originPosition = originPosition;

        grid = new T[width, height];
        //DebugTest();
    }

    private void DebugTest()
    {
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.green, 1000f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y ), Color.green, 1000f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, _height), GetWorldPosition(_width, _height), Color.green, 1000f);
        Debug.DrawLine(GetWorldPosition(_width, 0), GetWorldPosition(_width, _height), Color.green, 1000f);
    }

    public T this[int x, int y]
    {
        get => grid[x, y];
        set => grid[x, y] = value;       
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                yield return GetObject(x,y);
            }
        }
    }

    public int Width
    {
        get => _width; 
        set => _width = value; 
    }

    public int Height
    {
        get => _height; 
        set => _height = value; 
    }      

    public float CellWidth
    {
        get => _cellWidth; 
        set => _cellWidth = value; 
    }

    public float CellHeight
    {
        get => _cellHeight;
        set => _cellHeight = value;
    }

    public Vector3 OriginPosition
    {
        get => _originPosition;
        set => _originPosition = value;
    }

    public T[,] GetGrid
    {
        get => grid;        
    }

    public T[,] SetGrid
    {
        set => grid = value;
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x * _cellWidth, y * _cellHeight) + _originPosition;
    }

    public Vector3 GetWorldPositionSnapped(float x, float y)
    {        
        return new Vector3(Mathf.Round(x / _cellWidth) * _cellWidth, Mathf.Round(y / _cellHeight) * _cellHeight) + _originPosition;
    }

    public Vector2Int GetGridCoordinatesFromWorldPosition2D(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt((worldPosition - _originPosition).x / _cellWidth);
        int y = Mathf.FloorToInt((worldPosition - _originPosition).y / _cellHeight);
       
        return new Vector2Int(x, y);
    }

    public Vector3Int GetGridCoordinatesFromWorldPosition3D(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt((worldPosition - _originPosition).x / _cellWidth);
        int y = Mathf.FloorToInt((worldPosition - _originPosition).y / _cellHeight);
        int z = 0;

        return new Vector3Int(x, y, z);
    }

    public T GetObject(int x, int y)
    {
        if(x >= 0 && y >= 0 && x < Width && y < Height)
        {
            return grid[x, y];
        }
        else
        {
            return default(T);
        }
    }

    public void Clear()
    {
        grid = new T[_width, _height];
    }

    public List<T> GetAdjacentCells(int x, int y)
    {
        List<T> cells = new List<T>();
        for (int i = 0; i < neighbours.Length; i++)
        {
            int newX = x + neighbours[i].dx;
            int newY = y + neighbours[i].dy;
            if (newX >= 0 && newY >= 0 && newX < Width && newY < Height)
            {
                cells.Add(grid[newX, newY]);
            }
            else
            {
                continue;
            }
        }
        return cells;
    }

    public List<T> GetAdjacentCellsSimplified(int x, int y)
    {
        List<T> cells = new List<T>();
        for (int i = 0; i < neighboursSimple.Length; i++)
        {
            int newX = x + neighboursSimple[i].dx;
            int newY = y + neighboursSimple[i].dy;
            if (newX >= 0 && newY >= 0 && newX < Width && newY < Height)
            {
                cells.Add(grid[newX, newY]);
            }
            else
            {
                continue;
            }
        }
        return cells;
    }

    public struct Neighbours
    {
        public int dx;
        public int dy;

        public Neighbours(int dx, int dy)
        {
            this.dx = dx;
            this.dy = dy;
        }
    }
    public Neighbours[] neighbours = new Neighbours[] { new Neighbours(-1, -1), new Neighbours(-1, 0), new Neighbours(-1, 1), new Neighbours(0, -1), new Neighbours(0, 1), new Neighbours(1, -1), new Neighbours(1, 0), new Neighbours(1, 1) };
    //Ignores Diagonals
    public Neighbours[] neighboursSimple = new Neighbours[] { new Neighbours(1, 0), new Neighbours(0, -1), new Neighbours(0, 1), new Neighbours(-1, 0) };
}