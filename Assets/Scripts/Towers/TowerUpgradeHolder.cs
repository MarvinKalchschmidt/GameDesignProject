using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUpgradeHolder : MonoBehaviour
{
    [SerializeField] private GameObject _upgradeButtonHolder;
    [SerializeField] private UpgradeButton _damageUpgrade;
    [SerializeField] private UpgradeButton _speedUpgrade;
    [SerializeField] private Tooltip _tooltip;
    [SerializeField] private Vector3 _offset;

    private void Start()
    {
        _upgradeButtonHolder.SetActive(false);
    }

    public void ShowUpgradesForTower(Tower tower, Vector3 targetPosition)
    {
        _upgradeButtonHolder.transform.position = targetPosition + _offset;
        _upgradeButtonHolder.SetActive(true);

        _damageUpgrade.Tower = tower;
        _speedUpgrade.Tower = tower;
    }

    public void HideUpgradesForTower()
    {        
        _upgradeButtonHolder.SetActive(false);
        _tooltip.HideTooltip();
    }
}
