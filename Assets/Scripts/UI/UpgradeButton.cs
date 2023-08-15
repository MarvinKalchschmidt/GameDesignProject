using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private TowerUpgrade _towerUpgrade;
    [SerializeField] private Tooltip _tooltip;
    [SerializeField] private Tower _tower;    

    public Tower Tower { get => _tower; set => _tower = value; }
    public static event Action<string> OnDisplayMessage;

    private void Start()
    {
        if (_tooltip == null)
        {
            _tooltip = GameObject.FindGameObjectWithTag("TowerTooltip").GetComponent<Tooltip>();
        }
    }

    private void OnButtonBuyUpgrade(TowerUpgrade towerUpgrade)
    {
        if (CanAffordUpgrade(towerUpgrade))
        {
            if (_towerUpgrade.UpgradeType == UpgradeType.Damage)
            {
                Tower.Damage += _towerUpgrade.UpgradeAmount;
            }
            else if (_towerUpgrade.UpgradeType == UpgradeType.Speed)
            {
                Tower.AttackSpeed += _towerUpgrade.UpgradeAmount;
            }
        } 
        else
        {
            Debug.Log("Not Enough Money");
            OnDisplayMessage?.Invoke("Not enough money.");
            return;
        }        
    }

    private bool CanAffordUpgrade(TowerUpgrade towerUpgrade)
    {
        return GameManager.Instance.Money >= towerUpgrade.Cost;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Klick");
        OnButtonBuyUpgrade(_towerUpgrade);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _tooltip.ShowTooltip(_towerUpgrade, transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _tooltip.HideTooltip();
    }
}
