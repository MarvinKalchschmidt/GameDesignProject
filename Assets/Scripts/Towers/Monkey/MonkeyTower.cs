using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkeyTower : Tower
{
    [SerializeField] private Transform _projectileSummonPoint;
    //[SerializeField] private SpriteRenderer _rangeRadiusSpriteRenderer;
    [SerializeField] public TowerUpgradeHolder _towerUpgradeHolder;

    private float _cooldownTimer;
    private bool toggleUpgradeButton = false;

    private void Awake()
    {
        /*
        if (_rangeRadiusSpriteRenderer == null)
        {
            _rangeRadiusSpriteRenderer = _projectileSummonPoint.GetComponentInChildren<SpriteRenderer>();
        } */       
        if (_towerUpgradeHolder == null)
        {
            _towerUpgradeHolder = GameObject.FindGameObjectWithTag("TowerUpgradeHolder").GetComponent<TowerUpgradeHolder>();
        }
    }
    private void Update()
    {
        if (_towerIsPlaced && _enemiesWithinRange.Count > 0)
        {
            //Maybe add GameState to only check when Wave is generating
            _cooldownTimer -= Time.deltaTime;

            if (_cooldownTimer <= 0f)
            {
                _cooldownTimer = _cooldown;
                _animator.SetTrigger("attack");
            }
        }
    }

    protected override void Attack()
    {
        if(_enemiesWithinRange.Count > 0)
        {
            Enemy targetEnemy = GetEnemyWithinRadius();
            ShootBanana(_projectileSummonPoint.transform.position, targetEnemy, _attackSpeed, _damage);
        }        
    } 


    private Enemy GetEnemyWithinRadius()
    {
        Enemy closestEnemy = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (Enemy potentialTarget in _enemiesWithinRange)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                closestEnemy = potentialTarget;
            }
        }
        return closestEnemy;
    }

    private void ShootBanana(Vector3 bananaSummPosition, Enemy targetEnemy, float speed, float damage)
    {
        ProjectileSpawner.Instance.SummonBanana(bananaSummPosition, targetEnemy.transform.position, targetEnemy, speed, damage);
    }

    private void OnMouseDown()
    {
        /*
        if (_towerIsPlaced)
        {
            if (!toggleUpgradeButton)
            {
                //ShowRangeRadius();
                _towerUpgradeHolder.ShowUpgradesForTower(this, Camera.main.WorldToScreenPoint(transform.position));
                toggleUpgradeButton = true;

            }
            else
            {
                //HideRangeRadius();
                _towerUpgradeHolder.HideUpgradesForTower();
                toggleUpgradeButton = false;
            }    
        }*/
    }

    /*
    private void ShowRangeRadius()
    {
        _rangeRadiusSpriteRenderer.enabled = true;
    }

    private void HideRangeRadius()
    {
        _rangeRadiusSpriteRenderer.enabled = false;
    }*/   
}
