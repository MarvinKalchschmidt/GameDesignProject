using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private TowerInformation _towerInfo;
    [SerializeField] private Tooltip _tooltip;

    public static event Action<string> OnDisplayMessage;

    private void Start()
    {
        if (_tooltip == null)
        {
            _tooltip = GameObject.FindGameObjectWithTag("TowerTooltip").GetComponent<Tooltip>();
        }
    }


    public void OnButtonSpawnTower(TowerInformation towerInfo)
    {
        if (CanAffordTower(towerInfo))
        {
            TowerPlacementManager.Instance.OnButtonSpawnTower(towerInfo);
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
        return GameManager.Instance.Money >= towerInfo.Cost;        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnButtonSpawnTower(_towerInfo); 
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _tooltip.ShowTooltip(_towerInfo, transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _tooltip.HideTooltip();
    }
}
