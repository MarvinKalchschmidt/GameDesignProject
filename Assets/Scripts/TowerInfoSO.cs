using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerInfo", menuName = "Tower Information")]
public class TowerInfoSO : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private int _cost;
    [SerializeField] private TowerSpeed _speed;
    [SerializeField] private int _damage;
    [SerializeField][TextArea] private string _description;
    [SerializeField] private Tower _towerPrefab;
    [SerializeField] private int _buildWidth;
    [SerializeField] private int _buildHeight;

    public string Name { get => _name; } 
    public int Cost { get => _cost; } 
    public TowerSpeed Speed { get => _speed; } 
    public int Damage { get => _damage; } 
    public string Description { get => _description; } 
    public Tower GetPrefab { get => _towerPrefab; } 
    public int BuildWidth { get => _buildWidth; } 
    public int BuildHeight { get => _buildHeight; } 

    public List<Vector2Int> GetBuildingBoundsList(Vector2Int location)
    {
        List<Vector2Int> buildingBoundsList = new List<Vector2Int>();
        for (int x = 0; x < _buildWidth; x++)
        {
            for (int y = 0; y < _buildHeight; y++)
            {
                buildingBoundsList.Add(location + new Vector2Int(x, y));
            }

        }
        return buildingBoundsList;
    }
}

public enum TowerSpeed
{
    Slow = 0,
    Medium = 1,
    Fast = 2,
    SuperFast = 3
}
