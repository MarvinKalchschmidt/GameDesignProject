using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeopardTower : Tower
{
    [SerializeField] private LayerMask _enemyLayer;

    private float _cooldownTimer;
    private Enemy _currentEnemy;

    private void Awake() 
    {       
        if (_towerUpgradeHolder == null)
        {
            _towerUpgradeHolder = GameObject.FindGameObjectWithTag("TowerUpgradeHolder").GetComponent<TowerUpgradeHolder>();
        }
    }

    private void Update()
    {
        if (_towerIsPlaced && _targetDetection.DetectedTargets.Count > 0)
        {
            _cooldownTimer -= Time.deltaTime;

            if (_cooldownTimer <= 0f)
            {
                _cooldownTimer = _cooldown - (_attackSpeed / 10);
                _animator.SetTrigger("attack");
            }
        }
    }

    private bool EnemyInSight()
    {
        RaycastHit2D hit = Physics2D.CircleCast(_circleCollider.bounds.center, _circleCollider.radius * 2.3f, Vector2.right, 0, _enemyLayer);

        if(hit.collider != null)
        {
            _currentEnemy = hit.transform.GetComponent<Enemy>();
        }
        return hit.collider != null;       
    }

    protected override void Attack()
    {
        if (EnemyInSight())
        {
            _currentEnemy.TakeDamage(_damage);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_circleCollider.bounds.center, _circleCollider.radius * 2.3f);
    }
}
