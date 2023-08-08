using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour{

    [SerializeField] private Slider _slider;
    [SerializeField] private TreeOfLife _treeOfLife;

    void Start()
    {
        _slider.minValue = 0;
        _slider.maxValue = _treeOfLife.MaxHealth;

        _slider.value = _treeOfLife.CurrentHealth;
    }

    void OnEnable()
    {
        TreeOfLife.OnDamageTaken += SetHealthbar;
    }


    void OnDisable()
    {
        TreeOfLife.OnDamageTaken -= SetHealthbar;
    }

    public void SetHealthbar(float health)
    {
        _slider.value = health;
    }
}
