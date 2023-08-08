using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeOfLife : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 10000f;
    [SerializeField] private float _currentHealth;

    public static event Action<float> OnDamageTaken;
    
    public float MaxHealth { get => _maxHealth; set => _maxHealth = value; }
    public float CurrentHealth { get => _currentHealth; set => _currentHealth = value; }

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        OnDamageTaken?.Invoke(_currentHealth);
        if (_currentHealth <= 0)
        {
            //Trigger Game Over
        }
    }


    
}
