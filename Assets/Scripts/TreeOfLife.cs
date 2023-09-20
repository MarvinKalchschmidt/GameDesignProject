using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeOfLife : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 10000f;
    [SerializeField] private float _currentHealth;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _treeFullHealth;
    [SerializeField] private Sprite _treeSlightlyDamaged;
    [SerializeField] private Sprite _treeHeavilyDamaged;
    [SerializeField] private Sprite _treeDestroyed;
  

    public static event Action<float> OnDamageTaken;
    public static event Action OnFirstDamageTaken;
    public static event Action OnTreeHealthLow;
    public static event Action<GameOverState> OnTreeDead;

    private bool firstDamage = false;
    private bool treeIsLow = false;
    
    public float MaxHealth { get => _maxHealth; set => _maxHealth = value; }
    public float CurrentHealth { get => _currentHealth; set => _currentHealth = value; }

    private void Awake()
    {
        _currentHealth = _maxHealth;
        if(_spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        _spriteRenderer.sprite = _treeFullHealth;
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        ManageTreeSprite();
        OnDamageTaken?.Invoke(CurrentHealth);
        if (!firstDamage)
        {
            OnFirstDamageTaken?.Invoke();
        }
        if (CurrentHealth <= MaxHealth * 0.2f && !treeIsLow)
        {
            treeIsLow = true;
            OnTreeHealthLow?.Invoke();
        }
        if (CurrentHealth <= 0)
        {
            OnTreeDead?.Invoke(GameOverState.Loss);
        }
    }

    public void Heal(float heal)
    {
        if(CurrentHealth + heal <= _maxHealth)
        {
            CurrentHealth += heal;
        }
        else
        {
            CurrentHealth = MaxHealth;
        }
        ManageTreeSprite();
    }

    private void ManageTreeSprite()
    {
        if (CurrentHealth <= 0)
        {
            _spriteRenderer.sprite = _treeDestroyed;
        } 
        else if (CurrentHealth <= MaxHealth * 0.33f)
        {
            _spriteRenderer.sprite = _treeHeavilyDamaged;
        }
        else if (CurrentHealth <= MaxHealth * 0.66f)
        {
            _spriteRenderer.sprite = _treeSlightlyDamaged;
        }
        else
        {
            _spriteRenderer.sprite = _treeFullHealth;
        }

    }

}