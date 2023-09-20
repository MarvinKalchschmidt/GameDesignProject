using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class HealFruit : MonoBehaviour
{
    [SerializeField] private float _heal;
    [SerializeField] private float _timer;

    public static event Action<float> OnHealFruitClicked;
    public static event Action<HealFruit> OnHealFruitDestroyed;

    private void Start()
    {
        StartCoroutine(StartCountdown(_timer));
    }

    private IEnumerator StartCountdown(float countdowntime)
    {
        yield return new WaitForSeconds(countdowntime);
        DestroySelf();
    }

    private void OnMouseDown()
    {
        OnHealFruitClicked?.Invoke(_heal);
        DestroySelf();
    }

    private void DestroySelf()
    {
        OnHealFruitDestroyed?.Invoke(this);
        Destroy(this.gameObject);
    }    
}
