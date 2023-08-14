using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "New TowerInformation", menuName = "Tower Information")]
public class TowerInformation : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private int _cost;
    [SerializeField] private int _damage;
    [SerializeField] private int _attackSpeed;
    [SerializeField] [TextArea] private string _description;
    [SerializeField] private Tower _towerPrefab;
    [SerializeField] private int _buildWidth;
    [SerializeField] private int _buildHeight;

    public string Name { get => _name; }
    public int Cost { get => _cost; }
    public int Damage { get => _damage; }
    public int AttackSpeed { get => _attackSpeed; }
    public string Description { get => _description; }
    public Tower GetPrefab { get => _towerPrefab; }
    public int BuildWidth { get => _buildWidth; }
    public int BuildHeight { get => _buildHeight; }

    public List<Vector2Int> GetTowerBoundsList(Vector2Int location)
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

    public string GetTooltipInfo()
    {
        StringBuilder builder = new StringBuilder();

        builder.Append("<color=red>Damage: ").Append(_damage).Append("</color>").AppendLine();
        builder.Append("<color=red>Attack Speed: ").Append(_attackSpeed).Append("</color>").AppendLine();
        builder.Append("Description: ").Append(_description).AppendLine();
        builder.Append("<color=green>Cost: ").Append(_cost).Append(" Fruit").Append("</color>").AppendLine();

        return builder.ToString();        
    }
}