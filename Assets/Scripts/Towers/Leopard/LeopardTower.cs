using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeopardTower : Tower
{
    [SerializeField] public TowerUpgradeHolder _towerUpgradeHolder;
    private bool toggleUpgradeButton = false;
    [SerializeField] private LayerMask _enemyLayer;
    private float _cooldownTimer;

    private Enemy _currentEnemy;

    private void Awake() 
    {
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
        }
        if (_towerUpgradeHolder == null)
        {
            _towerUpgradeHolder = GameObject.FindGameObjectWithTag("TowerUpgradeHolder").GetComponent<TowerUpgradeHolder>();
        }
    }

    private void Update()
    {
        if (_towerIsPlaced && _enemiesWithinRange.Count > 0)
        {
            _cooldownTimer -= Time.deltaTime;

            if (_cooldownTimer <= 0f)
            {
                _cooldownTimer = _cooldown;
                _animator.SetTrigger("attack");
            }
        }
    }

    private bool EnemyInSight()
    {
        RaycastHit2D hit = Physics2D.CircleCast(_circleCollider.bounds.center, _circleCollider.radius, Vector2.right, 0, _enemyLayer);

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
    private void OnMouseDown()
    {
        if (_towerIsPlaced)
        {
            if (!toggleUpgradeButton)
            {
                //ShowRangeRadius();
                Debug.Log("Click on Tower");
                _towerUpgradeHolder.ShowUpgradesForTower(this, Camera.main.WorldToScreenPoint(transform.position));
                toggleUpgradeButton = true;

            }
            else
            {
                //HideRangeRadius();
                Debug.Log("Mouse off Tower");
                _towerUpgradeHolder.HideUpgradesForTower();
                toggleUpgradeButton = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_circleCollider.bounds.center, _circleCollider.radius*0.3f);
    }
}
