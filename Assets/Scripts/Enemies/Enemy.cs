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
    [SerializeField] private float _attackCooldown;
    [SerializeField] private int _killReward;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private Vector2 _direction = Vector2.left;
    [SerializeField] private BoxCollider2D _boxCollider;
    
    [SerializeField] private LayerMask _treeOfLifeLayer;

    private Rigidbody2D _rigidbody;    
    private Vector2 _velocity;
    private float _cooldownTimer = Mathf.Infinity;
    private Animator _animator;
    private TreeOfLife _treeOfLife;

    public static event Action<int> OnEnemyDiesFromDamage;

    public delegate void OnDestroyEnemy(Enemy enemy);
    public OnDestroyEnemy DestroyEnemy;

    private bool _alive;

    public int DamageDealt { get => _damage; set => _damage = value; }
    public int KillReward { get => _killReward; set => _killReward = value; }

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
        if(_boxCollider == null) {
            _boxCollider = GetComponent<BoxCollider2D>();
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

    private bool CheckForTarget()
    {
        RaycastHit2D hit = Physics2D.BoxCast(_boxCollider.bounds.center + transform.right * transform.localScale.x, _boxCollider.bounds.size, 0, Vector2.left, 0, _treeOfLifeLayer);
        if(hit.collider != null)
        {
            _treeOfLife = hit.transform.GetComponent<TreeOfLife>();
        }
        return (hit.collider != null);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_boxCollider.bounds.center + transform.right * transform.localScale.x, _boxCollider.bounds.size);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BananaProjectile"))
        {
            TakeDamage(other.gameObject.GetComponent<BananaProjectile>().BananaDamage);
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