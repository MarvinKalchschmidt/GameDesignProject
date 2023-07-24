using System;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuildManager : Singleton<TowerBuildManager>
{ /*
    //[SerializeField] private List<TowerStoreObject> _towerStoreObjects;
    [SerializeField] private TowerStoreObject[] _towerStoreObjects;
    private Grid<int> _grid;
    private BuildState _buildState;
    private Tower _currentTower;
    private TowerInfoSO _currentTowerInfo;
    public List<Tower> _placedTowers = new List<Tower>();

    public static event Action<int> OnTowerPlaced;
    public static event Action<string> OnDisplayMessage;
    public BuildState BuildState { get => _buildState; private set => _buildState = value; }   
    public List<Tower> PlacedTowers { get => _placedTowers; private set => _placedTowers = value; }    

    private void Start()
    {
        _grid = TDGameManager.Instance.Grid;
        _buildState = BuildState.NotPlacingTower;
        _towerStoreObjects = GameObject.FindGameObjectWithTag("TowerStore").GetComponentsInChildren<TowerStoreObject>();
    }   

    private void Update()
    {
        if (BuildState == BuildState.PlacingTower && _currentTower != null)
        {
            _currentTower.transform.position = GetMouseWorldPosition() + new Vector3(-0.75f, -0.5f, 0);

            if (BoundsCheck3D(GetMouseWorldPosition())) {
                bool allTilesUnoccupied = true;
                Vector2Int towerPosition = _grid.GetGridCoordinatesFromWorldPosition2D(GetMouseWorldPosition());
                List<Vector2Int> buildingBoundsList = _currentTowerInfo.GetBuildingBoundsList(towerPosition);

                foreach (Vector2Int pos in buildingBoundsList)
                {
                    if (OccupiedTileCheck(_grid[pos.x, pos.y]))
                    {
                        allTilesUnoccupied = false;
                        break;
                    }
                }            
           
            /* Attempted to make a hover effect while placing a tower to visualize the validity of a current position, but couldnt be bothered to finish it
             * foreach(Vector2Int pos in buildingBoundsList)
            {
                if(allTilesUnoccupied)
                {
                    _grid[pos.x, pos.y].ActivateHoverEffect(new Color(59, 219, 57, 83));
                }
                else
                {
                    _grid[pos.x, pos.y].ActivateHoverEffect(new Color(255, 17, 0, 83));
                }
            }*/ 
/*
                if (Input.GetMouseButtonDown(0))
                { 
                    if (allTilesUnoccupied)
                    {                    
                        BuildState = BuildState.NotPlacingTower;
                        Vector3 towerLocation = new Vector3(towerPosition.x, towerPosition.y);
                        _currentTower.transform.position = towerLocation;
                        _currentTower.BuildingBounds = buildingBoundsList;
                        _currentTower.TowerIsPlaced = true;
                        _placedTowers.Add(_currentTower);                      
                        OnTowerPlaced?.Invoke(-1 * _currentTower.Cost);
                        foreach (Vector2Int pos in buildingBoundsList)
                        {
                            _grid[pos.x, pos.y].SetTower(_currentTower);
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
    
    public void OnButtonSpawnTower(TowerInfoSO towerInfo)
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

    private bool CanAffordTower(TowerInfoSO towerInfo)
    {
        return TDGameManager.Instance.Money >= towerInfo.Cost;
    }

    private bool OccupiedTileCheck(Tile tile)
    {
        return tile.IsOccupied || tile.TileType == TileType.Path || tile.TileType == TileType.Obstacle;   
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

    public void UnhoverOtherStoreObjects(TowerInfoSO towerInfo)
    {
        foreach(TowerStoreObject towerStoreObject in _towerStoreObjects)
        {
            if(towerInfo.Name == towerStoreObject.GetTowerInfo.Name)
            {
                continue;
            }
            else
            {
                towerStoreObject.Hovered = false;
            }
        }
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
    }   */
    
}
public enum BuildState
{
    NotPlacingTower,
    PlacingTower
}
