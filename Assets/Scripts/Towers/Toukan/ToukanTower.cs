using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToukanTower : Tower
{
    [SerializeField] private LayerMask _enemyLayer;  
    [SerializeField] private Vector3 _toucanOffset = new Vector3(10, 0, 0);  
    private float _cooldownTimer;

    private ToukanState _toukanState;  
    private Vector3 direction = new Vector3(0, -1, 0);
    private Vector3 _summonPos;
    private Vector3 _targetPos;

    public ToukanState ToukanState { get => _toukanState; }

    private Enemy _currentEnemy;
    public float duration = 1.0f;
    private float elapsedTime = 0.0f;

  
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

    private void Start()
    {
        _toukanState = ToukanState.Idling;       
    }

    private void FixedUpdate()
    {
        if (_towerIsPlaced)
            {
            _summonPos = TowerStartLocation;
            _targetPos = TowerStartLocation + _toucanOffset;
            if (_toukanState == ToukanState.Diving)                 
            {
                Nosedive(_summonPos,_targetPos, direction);

                if (transform.position == _targetPos) 
                {
                    elapsedTime = 0f;
                    _toukanState = ToukanState.Returning;
                }
                return;
            }

            if(_toukanState == ToukanState.Returning)
            {
                Nosedive(_targetPos, _summonPos, direction);

                if (transform.position == _summonPos)
                {
                    elapsedTime = 0f;
                    _toukanState = ToukanState.Idling;
                }
                return;
            }     
        }
    }

    private void Nosedive(Vector3 startPos, Vector3 endPos, Vector3 direction)
    {
        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / duration);
        
        Vector3 center = (startPos + endPos) * .5f;
        center -= direction;

        transform.position = Vector3.Slerp(startPos - center, endPos - center, t) + center;
    }   

    private bool EnemyInSight()
    {
        RaycastHit2D hit = Physics2D.CircleCast(_circleCollider.bounds.center, _circleCollider.radius * 2.3f, Vector2.right, 0, _enemyLayer);

        if (hit.collider != null)
        {
            _currentEnemy = hit.transform.GetComponent<Enemy>();
        }
        return hit.collider != null;
    }

    protected override void Attack()
    {
        _toukanState = ToukanState.Diving;
        /*if (EnemyInSight())
        {
            Debug.Log("Enemy in sight");
            _currentEnemy.TakeDamage(_damage);
        }*/
    }  

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_circleCollider.bounds.center, _circleCollider.radius * 2.3f);
    }
}


public enum ToukanState
{
    Diving = 0,
    Returning = 1,
    Idling = 2
}
