using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeOfLife : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 10000f;
    [SerializeField] private float _currentHealth;

    public static event Action<float> OnDamageTaken;
    public static event Action<bool> OnTreeLifeLow;
    public static event Action<GameOverState> OnTreeDead;
    
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
        if (_currentHealth <= MaxHealth * 0.2f)
        {
            OnTreeDead?.Invoke(GameOverState.Loss);
        }
        if (_currentHealth <= 0)
        {
            OnTreeDead?.Invoke(GameOverState.Loss);
        }
    }    
}