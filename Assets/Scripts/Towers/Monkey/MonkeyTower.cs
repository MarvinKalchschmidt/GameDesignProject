using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkeyTower : Tower
{
    protected float _cooldownTimer;   

    private void Update()
    {
        Debug.Log(_enemiesWithinRange.Count);
        Attack();
    }

    protected override void Attack()
    {
        Debug.Log("Test");
        if (_towerIsPlaced && _enemiesWithinRange.Count > 0)
        {
            //Maybe add GameState to only check when Wave is generating
            _cooldownTimer -= Time.deltaTime;

            if (_cooldownTimer <= 0f)
            {
                Enemy targetEnemy = GetEnemyWithinRadius();
                _cooldownTimer = _cooldown;
                ShootBanana(_projectileSummonPoint.transform.position, targetEnemy, _attackSpeed, _damage);
            }
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
        if (_towerIsPlaced)
        {
            //ShowRangeRadius();
        }
    }

    private void OnMouseExit()
    {
        //HideRangeRadius();
    }
}
