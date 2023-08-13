using System;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacementManager : Singleton<TowerPlacementManager>
{
    [SerializeField] private int _gridWidth;
    [SerializeField] private int _gridHeight;
    [SerializeField] private float _cellSize;

    [SerializeField] Vector2 _minBounds;
    [SerializeField] Vector2 _maxBounds;


    private CustomGrid<GridTowerObject> _grid;
    private BuildState _buildState;
    private Tower _currentTower;
    private TowerInformation _currentTowerInfo;
    public List<Tower> _placedTowers = new List<Tower>();

    public static event Action<int> OnTowerPlaced;
    public static event Action<string> OnDisplayMessage;

    public BuildState BuildState { get => _buildState; private set => _buildState = value; }
    public List<Tower> PlacedTowers { get => _placedTowers; private set => _placedTowers = value; }

    private void Start()
    { 
        _grid = new CustomGrid<GridTowerObject>(_gridWidth, _gridHeight, _cellSize, transform.position);
        FillGrid();
        _buildState = BuildState.NotPlacingTower;
    }

    private void FixedUpdate()
    {
        if (BuildState != BuildState.PlacingTower || _currentTower == null)
        {
            return;
        }

        Vector3 mouseWorldPosition = GetMouseWorldPosition();
        if (_currentTower != null)
        {
            _currentTower.transform.position = mouseWorldPosition;
        }    
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if(_currentTower != null)
            {
                DestroyTower(_currentTower);
            }
        }

        if (BuildState != BuildState.PlacingTower || _currentTower == null)
        {
            return;
        }

        Vector3 mouseWorldPosition = GetMouseWorldPosition();

        if (!BoundsCheck3D(mouseWorldPosition))
        {
            return;
        }

        Vector2Int gridPosition = _grid.GetGridCoordinatesFromWorldPosition2D(mouseWorldPosition);
        List<Vector2Int> towerBoundsList = _currentTowerInfo.GetTowerBoundsList(gridPosition);
        bool allTilesUnoccupied = true;

        foreach (Vector2Int pos in towerBoundsList)
        {
            if (BoundsCheck2D(pos) && !_grid.GetObject(pos.x, pos.y).OccupiedCheck())
            {
                allTilesUnoccupied = false;
                break;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (allTilesUnoccupied)
            {
                BuildState = BuildState.NotPlacingTower;
                Vector3 towerLocation = _grid.GetWorldPosition(gridPosition.x, gridPosition.y);
                _currentTower.transform.position = towerLocation;
                _placedTowers.Add(_currentTower);

                foreach (Vector2Int towerBound in towerBoundsList)
                {
                    if (BoundsCheck2D(towerBound))
                    {
                        Debug.Log("SUCCESS: " + towerBound + " " + BoundsCheck2D(towerBound));
                        _grid.GetObject(towerBound.x, towerBound.y).SetTower(_currentTower);
                    }
                    else
                    {
                        Debug.Log("FAILED: " + towerBound + " " + BoundsCheck2D(towerBound));
                    }
                }
            }
            else
            {
                OnDisplayMessage?.Invoke("Can't build here.");
                Debug.Log("Can't Build Here");
            }           
        }

       
       

        /*
        if (BuildState == BuildState.PlacingTower && _currentTower != null)
        {
            Vector3 mouseWorldPosition = GetMouseWorldPosition();
            _currentTower.transform.position = mouseWorldPosition;

            if (BoundsCheck3D(mouseWorldPosition))
            {
                Vector2Int gridPosition = _grid.GetGridCoordinatesFromWorldPosition2D(mouseWorldPosition);
                List<Vector2Int> towerBoundsList = _currentTowerInfo.GetTowerBoundsList(gridPosition);

                bool allTilesUnoccupied = true;

                foreach (Vector2Int pos in towerBoundsList)
                {
                    if (BoundsCheck2D(pos) && !_grid.GetObject(pos.x, pos.y).OccupiedCheck())
                    {
                        allTilesUnoccupied = false;
                        break;
                    }
                }
        
                if (Input.GetMouseButtonDown(0))
                {
                    if (allTilesUnoccupied)
                    {
                        BuildState = BuildState.NotPlacingTower;
                        Vector3 towerLocation = _grid.GetWorldPosition(gridPosition.x, gridPosition.y);
                        _currentTower.transform.position = towerLocation;
                        _placedTowers.Add(_currentTower);

                        foreach (Vector2Int towerBound in towerBoundsList)
                        {
                            _grid.GetObject(towerBound.x, towerBound.y).SetTower(_currentTower);
                        }
                    }
                    else
                    {
                        OnDisplayMessage?.Invoke("Can't build here.");
                        Debug.Log("Can't Build Here");
                    }
                }

                if (Input.GetMouseButtonDown(1))
                {                
                    DestroyTower(_currentTower);
                }
            }
        } */
    }

    private void FillGrid()
    {
        for (int x = 0; x < _grid.Width; x++)
        {
            for (int y = 0; y < _grid.Height; y++)
            {
                _grid[x, y] = new GridTowerObject(x, y);
            }
        }
    }
    
    public void OnButtonSpawnTower(TowerInformation towerInfo)
    {      
        _currentTowerInfo = towerInfo;      

        if (BuildState == BuildState.PlacingTower && _currentTower != null)
        {
            DestroyTower(_currentTower);
        }

        BuildState = BuildState.PlacingTower;
        _currentTower = SpawnTower(towerInfo.GetPrefab, GetMouseWorldPosition());
        _currentTower.Cost = towerInfo.Cost;
        //_currentTower.ShowRangeRadius();
    }

    private bool BoundsCheck3D(Vector3 vectorToCheck)
    {
        return vectorToCheck.x >= 0 && vectorToCheck.x < _grid.Width && vectorToCheck.y >= 0 && vectorToCheck.y < _grid.Height;
    }

    private bool BoundsCheck2D(Vector2Int vectorToCheck)
    {
        return vectorToCheck.x >= 0 && vectorToCheck.x < _grid.Width && vectorToCheck.y >= 0 && vectorToCheck.y < _grid.Height;
    }

    public Tower SpawnTower(Tower towerToSpawn, Vector3 position)
    {
        return Instantiate(towerToSpawn, position, Quaternion.identity);       
    }

    //Destroy Tower GameObject
    private void DestroyTower(Tower towerToDestroy) 
    {
        _placedTowers.Remove(towerToDestroy);
        Destroy(towerToDestroy.gameObject);
    } 

    public void ClearTowers()
    {
        foreach(Tower tower in _placedTowers)
        {
           
            Destroy(tower.gameObject);
        }
        _placedTowers.Clear();       
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0;
        return mouseWorldPosition;
    }
 }

public class GridTowerObject
{
    private int x;
    private int y;
    private Tower _tower;

    public GridTowerObject(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public void SetTower(Tower tower)
    {
        this._tower = tower;
    }

    public bool OccupiedCheck()
    {
        return _tower == null;
    }

    public void ClearTower()
    {
        _tower = null;
    }

    public override string ToString()
    {
        return "(" + x + ", " + y + "): " + _tower.TowerType;
    }
}

public enum BuildState
{
    NotPlacingTower,
    PlacingTower
}
