using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _currentHealth;
    [SerializeField] private int _damage;
    [SerializeField] private int _killReward;

    [SerializeField] private float _speed = 1f;
    [SerializeField] private Vector2 _direction = Vector2.left;

    private Rigidbody2D _rigidbody;
    private Vector2 _velocity;

    public static event Action<int> OnEnemyDiesFromDamage;

    public delegate void OnDestroyEnemy(Enemy enemy);
    public OnDestroyEnemy DestroyEnemy;

    private bool _alive;

    public int DamageDealt { get => _damage; set => _damage = value; }
    public int KillReward { get => _killReward; set => _killReward = value; }

     private void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable() {
        InitEnemy();
        _rigidbody.WakeUp();
    }

    private void OnDisable() {
        _rigidbody.velocity = Vector2.zero;
        _rigidbody.Sleep();
    }

    private void FixedUpdate() {
        _velocity.x = _direction.x * _speed;
        _velocity.y += Physics2D.gravity.y * Time.fixedDeltaTime; 

        _rigidbody.MovePosition(_rigidbody.position + _velocity  * Time.fixedDeltaTime);
    }

    private void InitEnemy()
    {
        _currentHealth = _maxHealth;
        _alive = true;
    }

    public void TakeDamage(float damageToTake)
    {
        _currentHealth -= damageToTake;
        if(_currentHealth <= 0 && _alive)
        {            
            OnEnemyDiesFromDamage?.Invoke(this.KillReward);
            DestroySelf();           
        }
    }

    public void DestroySelf()
    {
        _alive = false;
        DestroyEnemy?.Invoke(this);
    }
}

