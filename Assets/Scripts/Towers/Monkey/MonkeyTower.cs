using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkeyTower : Tower
{
    [SerializeField] private Transform _projectileSummonPoint;
    //[SerializeField] private SpriteRenderer _rangeRadiusSpriteRenderer;
    [SerializeField] protected float _projectileSpeed;

    public float ProjectileSpeed { get => _projectileSpeed; set => _projectileSpeed = value; }

    private float _cooldownTimer;

   
    private void Update()
    {
        if (_towerIsPlaced && _targetDetection.DetectedTargets.Count > 0)
        {
            //Maybe add GameState to only check when Wave is generating
            _cooldownTimer -= Time.deltaTime;

            if (_cooldownTimer <= 0f)
            {
                _cooldownTimer = _cooldown - (_attackSpeed / 10);
                _animator.SetTrigger("attack");
            }
        }
    }

    protected override void Attack()
    {
        if(_targetDetection.DetectedTargets.Count > 0)
        {
            Enemy targetEnemy = GetEnemyWithinRadius();
            ShootBanana(_projectileSummonPoint.transform.position, targetEnemy, _projectileSpeed, _damage);
        }        
    } 


    private Enemy GetEnemyWithinRadius()
    {
        Enemy closestEnemy = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (GameObject potentialTarget in _targetDetection.DetectedTargets)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                closestEnemy = potentialTarget.GetComponent<Enemy>();
            }
        }
        return closestEnemy;
    }

    private void ShootBanana(Vector3 bananaSummPosition, Enemy targetEnemy, float speed, float damage)
    {
        ProjectileSpawner.Instance.SummonBanana(bananaSummPosition, targetEnemy.transform.position, targetEnemy, speed, damage);
    }

    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_circleCollider.bounds.center, _circleCollider.radius * 2.3f);
    }
}
