using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private GameObject _toolTip;
    [SerializeField] private RectTransform _toolTipRect;
    [SerializeField] private TextMeshProUGUI _headerText;
    [SerializeField] private TextMeshProUGUI _infoText;
    [SerializeField] private Vector3 _offset;

    private void Awake()
    {
        _toolTip.SetActive(false);
    }

    public void ShowTooltip(TowerInformation towerInfo, Vector3 targetPosition)
    {
        _headerText.text = towerInfo.Name;
        _infoText.text = towerInfo.GetTooltipInfo();

        _toolTip.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(_toolTipRect);

        Vector3 tooltipPosition = targetPosition + _offset;
        _toolTipRect.transform.position = tooltipPosition;
    }

    public void ShowTooltip(TowerUpgrade towerUpgrade, Vector3 targetPosition)
    {
        _headerText.text = towerUpgrade.Name;
        _infoText.text = towerUpgrade.GetTooltipInfo();

        _toolTip.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(_toolTipRect);

        Vector3 tooltipPosition = targetPosition + _offset;
        _toolTipRect.transform.position = tooltipPosition;
    }

    public void HideTooltip()
    {
        _toolTip.SetActive(false);
    }
}
