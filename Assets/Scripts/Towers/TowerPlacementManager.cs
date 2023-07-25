using System;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacementManager : MonoBehaviour
{
    [SerializeField] private int _gridWidth = 10;
    [SerializeField] private int _gridHeight = 10;
    [SerializeField] private float _cellSize = 10f;

    private CustomGrid<GridTowerObject> _grid;
    private BuildState _buildState;
    private Tower _currentTower;
    public TowerInformation _currentTowerInfo;
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


    private void Update()
    {
        if (BuildState == BuildState.PlacingTower && _currentTower != null)
        {
            _currentTower.transform.position = GetMouseWorldPosition();

            if (!BoundsCheck3D(GetMouseWorldPosition()))
            {
                Vector2Int gridPosition = _grid.GetGridCoordinatesFromWorldPosition2D(GetMouseWorldPosition());
                List<Vector2Int> towerBoundsList = _currentTowerInfo.GetTowerBoundsList(gridPosition);

                bool allTilesUnoccupied = true;

                foreach (Vector2Int pos in towerBoundsList)
                {
                    if (!_grid.GetObject(pos.x, pos.y).OccupiedCheck())
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

                        //Tower tower = SpawnTower(_currentTowerInfo.GetPrefab, _grid.GetWorldPosition(gridPosition.x, gridPosition.y));
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
        }   
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
        if (CanAffordTower(towerInfo))
        {
            _currentTowerInfo = towerInfo;
            switch (BuildState)
            {
                case BuildState.NotPlacingTower:
                    BuildState = BuildState.PlacingTower;
                    _currentTower = SpawnTower(towerInfo.GetPrefab, GetMouseWorldPosition());
                    _currentTower.Cost = towerInfo.Cost;
                    _currentTower.ShowRangeRadius();                    
                    break;
                case BuildState.PlacingTower:
                    if(_currentTower != null) {
                        DestroyTower(_currentTower);
                    }
                    _currentTower = SpawnTower(towerInfo.GetPrefab, GetMouseWorldPosition());                    
                    _currentTower.Cost = towerInfo.Cost;
                    _currentTower.ShowRangeRadius();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }            
        }
        else
        {
            Debug.Log("Not Enough Money");
            OnDisplayMessage?.Invoke("Not enough money.");
            return;
        }        
    }

    private bool CanAffordTower(TowerInformation towerInfo)
    {
        //return TDGameManager.Instance.Money >= towerInfo.Cost;
        return true;
    }   

    private bool BoundsCheck3D(Vector3 vectorToCheck)
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
