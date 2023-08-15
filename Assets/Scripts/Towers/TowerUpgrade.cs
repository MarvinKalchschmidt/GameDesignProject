using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "New TowerUpgrade", menuName = "Tower Upgrade")]

public class TowerUpgrade : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] [TextArea] private string _description;
    [SerializeField] private int _cost;
    [SerializeField] private float _upgradeAmount;
    [SerializeField] private UpgradeType _upgradeType;

    public string Name { get => _name; }
    public string Description { get => _description; }
    public int Cost { get => _cost; }
    public float UpgradeAmount { get => _upgradeAmount; }
    public UpgradeType UpgradeType { get => _upgradeType; }

    public string GetTooltipInfo()
    {
        StringBuilder builder = new StringBuilder();

        builder.Append("<color=red>Increases ").Append(_upgradeType.ToString()).Append(" by ").Append(_upgradeAmount).Append(" Points.</color>").AppendLine();
        builder.Append("Description: ").Append(_description).AppendLine();
        builder.Append("<color=green>Cost: ").Append(_cost).Append(" Fruit").Append("</color>").AppendLine();

        return builder.ToString();
    }
}

public enum UpgradeType
{
    Damage = 0,
    Speed = 1
}
