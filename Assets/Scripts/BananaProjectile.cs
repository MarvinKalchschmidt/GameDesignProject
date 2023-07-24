using System;
using UnityEngine;

public class BananaProjectile : MonoBehaviour
{
    private float _arrowSpeed;
    private float _arrowDamage;
    public Enemy _targetEnemy;

    public static event Action<BananaProjectile> OnArrowHitEnemy;
    public delegate void OnDestroyArrow(BananaProjectile arrow);
    public OnDestroyArrow DestroyArrow;
  
    public void SummonArrow(Vector3 summonPos, Enemy enemy, float arrowSpeed, float arrowDamage)
    {
        this.transform.position = summonPos;
        this._targetEnemy = enemy;
        this._arrowSpeed = arrowSpeed;
        this._arrowDamage = arrowDamage;
    }        

    private void Update()
    {
        if(_targetEnemy != null)
        {
            Vector3 targetPosition = _targetEnemy.transform.position;              
            Vector3 direction = (targetPosition - transform.position).normalized;
            ArrowMovementAndRotation(direction, _arrowSpeed);

            if(CheckForCollision(transform.position, targetPosition))
            {
                OnArrowHitEnemy?.Invoke(this);
                _targetEnemy.TakeDamage(_arrowDamage);
                DestroySelf();
            }
        }        
        else
        {
            DestroySelf();
        }        
    }         

    private void ArrowMovementAndRotation(Vector3 direction, float movementSpeed)
    {
        transform.position += direction * movementSpeed * Time.deltaTime;         
        transform.rotation = Quaternion.Euler(0, 0, ConvertDirectionToAngle(direction));
    }

    private bool CheckForCollision(Vector3 projectilePos, Vector3 targetPos)
    {
        return Vector3.Distance(projectilePos, targetPos) < .5f;       
    }

    private float ConvertDirectionToAngle(Vector3 dir)
    {        
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 180f;
        if (angle < 0) angle += 360f;
        return angle;
    }

    private void DestroySelf()
    {
        DestroyArrow?.Invoke(this);
    }
}
