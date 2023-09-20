using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerPlacementManager : Singleton<TowerPlacementManager>
{
    [SerializeField] private int _gridWidth;
    [SerializeField] private int _gridHeight;
    [SerializeField] private float _cellSize;
    [SerializeField] private Tilemap _tilemap;

    [SerializeField] private Vector2 _minBounds;
    [SerializeField] private Vector2 _maxBounds;

    private CustomGrid<GridTowerObject> _grid;
    private BuildState _buildState;
    private Tower _currentTower;
    private TowerInformation _currentTowerInfo;
    public List<Tower> _placedTowers = new List<Tower>();
    [SerializeField] private int _maxAmountOfTowers;

    public static event Action<int> OnTowerPlaced;
    public static event Action<string> OnDisplayMessage;

    public BuildState BuildState { get => _buildState; private set => _buildState = value; }
    public List<Tower> PlacedTowers { get => _placedTowers; private set => _placedTowers = value; }
    public CustomGrid<GridTowerObject> GetGrid { get => _grid; private set => _grid = value; }
    public int MaxAmountOfTowers { get => _maxAmountOfTowers; set => _maxAmountOfTowers = value; }


    private void Awake()
    { 
        _grid = new CustomGrid<GridTowerObject>(_gridWidth, _gridHeight, _cellSize, transform.position);
        FillGrid();      

        if (_tilemap == null)
        {
            _tilemap = gameObject.GetComponent<Tilemap>();
        }
        _tilemap.InitTilemap(_grid);
        HideTilemap();
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
            _currentTower.transform.position = Vector3.Lerp(_currentTower.transform.position, _grid.GetWorldPositionSnapped(mouseWorldPosition.x - _grid.CellWidth/2, mouseWorldPosition.y - _grid.CellHeight / 2), Time.fixedDeltaTime * 10f);
        }    
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if(_currentTower != null && !_currentTower.TowerIsPlaced)
            {
                DestroyTower(_currentTower);
            }
            HideTilemap();
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
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (allTilesUnoccupied)
            {
                BuildState = BuildState.NotPlacingTower;
                Vector3 towerLocation = _grid.GetWorldPosition(gridPosition.x, gridPosition.y);
                InitTower(_currentTower, towerLocation);

                foreach (Vector2Int towerBound in towerBoundsList)
                {
                    if (BoundsCheck2D(towerBound))
                    {
                        _grid.GetObject(towerBound.x, towerBound.y).SetTower(_currentTower);
                        HideTilemap();
                    }
                    else
                    {
                        OnDisplayMessage?.Invoke("Can't build here.");
                    }
                }
            }
            else
            {
                OnDisplayMessage?.Invoke("Can't build here.");
                Debug.Log("Can't Build Here");
            }           
        }       
    }

    private void InitTower(Tower tower, Vector3 towerLocation)
    {       
        tower.InitTower(towerLocation, _currentTowerInfo.Damage);
        OnTowerPlaced.Invoke(_currentTowerInfo.Cost * -1);
        _placedTowers.Add(tower);
    }

    private void FillGrid()
    {
        for (int x = 0; x < _grid.Width; x++)
        {
            for (int y = 0; y < _grid.Height; y++)
            {
                GridTowerObject gridTowerObject = new GridTowerObject(x, y);

                if(x < _minBounds.x || x >= _grid.Width - _maxBounds.x || y < _minBounds.y || y >= _grid.Height - _maxBounds.y)
                {
                    gridTowerObject.Occupied = true;
                }

                _grid[x, y] = gridTowerObject;
            }
        }
    }
    
    public void OnButtonSpawnTower(TowerInformation towerInfo)
    {
        if (!GameManager.Instance.ControllsEnabled)
        {
            return;
        }

        _currentTowerInfo = towerInfo;      

        if (BuildState == BuildState.PlacingTower && _currentTower != null)
        {
            DestroyTower(_currentTower);
        }

        BuildState = BuildState.PlacingTower;
        ShowTilemap();
        _currentTower = SpawnTower(towerInfo.GetPrefab, GetMouseWorldPosition());
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

    private void ShowTilemap()
    {
        _tilemap.gameObject.SetActive(true);
        _tilemap.UpdateTileMap();
    }
    
    private void HideTilemap()
    {
        _tilemap.UpdateTileMap();
        _tilemap.gameObject.SetActive(false);
    }

    public Tower SpawnTower(Tower towerToSpawn, Vector3 position)
    {
        return Instantiate(towerToSpawn, position, Quaternion.identity, transform);       
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
    private bool _occupied;
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

    public bool Occupied { get => _occupied; set { _occupied = value; } }

    public bool OccupiedCheck()
    {
        return _tower == null && !Occupied;
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

public enum TilemapSprite
{
    None = 0,
    Green = 1,
    Red = 2
}