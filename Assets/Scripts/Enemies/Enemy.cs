using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyType __enemyType;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _currentHealth;
    [SerializeField] private int _damage;
    [SerializeField] private float _attackCooldown;
    [SerializeField] private int _killReward;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private Vector2 _direction = Vector2.left;
    [SerializeField] private TargetDetectionUtil _targetDetection;
    [SerializeField] private float _immunityTime = 1.5f;
    [SerializeField] private bool _isImmune = false;

    private Rigidbody2D _rigidbody;    
    private Vector2 _velocity;
    private Animator _animator;
    private TreeOfLife _treeOfLife;

    public static event Action<int> OnEnemyDiesFromDamage;

    public delegate void OnDestroyEnemy(Enemy enemy);
    public OnDestroyEnemy DestroyEnemy;

    private bool _alive;
    private float _cooldownTimer = Mathf.Infinity;
    private float _immunityTimer = 0.5f;

    public int DamageDealt { get => _damage; set => _damage = value; }
    public int KillReward { get => _killReward; set => _killReward = value; }
    public EnemyType EnemyType { get => __enemyType; set => __enemyType = value; }

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
        if (_targetDetection == null)
        {
            _targetDetection = GetComponent<TargetDetectionUtil>();
        }
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
        }
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
        _rigidbody.MovePosition(_rigidbody.position + _velocity  * Time.fixedDeltaTime);

        _cooldownTimer += Time.fixedDeltaTime;

        if (CheckForTarget())
        {
            if (_cooldownTimer >= _attackCooldown)
            {
                _cooldownTimer = 0;
                _animator.SetTrigger("attack");
            }
        }

        if (_isImmune)
        {
            _immunityTimer -= Time.deltaTime;

            if (_immunityTimer <= 0f)
            {
                _isImmune = false;
            }
        }
    }

    private void InitEnemy()
    {
        _currentHealth = _maxHealth;
        _treeOfLife = null;
        _alive = true;
    }

    public void TakeDamage(float damageToTake)
    {
        if (!_isImmune)
        {
            _currentHealth -= damageToTake;
            _isImmune = true;
            _immunityTimer = _immunityTime;
        }

        if (_currentHealth <= 0 && _alive)
        {            
            OnEnemyDiesFromDamage?.Invoke(this.KillReward);
            DestroySelf();           
        }
    }    

    public void DestroySelf()
    {
        _alive = false;
        Physics2D.IgnoreLayerCollision(6, 7, false);
        DestroyEnemy?.Invoke(this);
    }

    private bool CheckForTarget()
    {
        if (_targetDetection.DetectedTargets.Count > 0)
        {
            _treeOfLife = _targetDetection.DetectedTargets[0].GetComponent<TreeOfLife>();
            return true;
        }
        return false;
    }       

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BananaProjectile"))
        {
            TakeDamage(other.gameObject.GetComponent<BananaProjectile>().BananaDamage);
        }

        if (other.CompareTag("Tower") && other.gameObject.TryGetComponent(out ToukanTower toukanTower))
        {
            if(toukanTower.ToukanState != ToukanState.Idling)
            {
                TakeDamage(toukanTower.Damage);
            }
        }
    }      
  
    public void DoDamage()
    {
        if (CheckForTarget())
        {
            _treeOfLife.TakeDamage(_damage);
        }
    }
}

public enum EnemyType
{
    Duo = 0,
    Chainsaw = 1,
    Excavator = 2
}