using System;
using UnityEngine;

public class BananaProjectile : MonoBehaviour
{
    private Vector3 _summonPos;
    private Vector3 _targetPos;
    public Enemy _targetEnemy;

    private ProjectileState _projectileState;
    [SerializeField] private float _bananaSpeed;
    [SerializeField] private float _bananaDamage;

    public float BananaDamage { get => _bananaDamage; set => _bananaDamage = value; }


    public delegate void OnDestroyBanana(BananaProjectile banana);
    public OnDestroyBanana DestroyBanana;

    private float startTime;
    private Vector3 direction1 = new Vector3(0, 5, 0);
    private Vector3 direction2 = new Vector3(0, -5, 0);
    private bool throwing = true;


    public void SummonBanana(Vector3 summonPos, Vector3 targetPos, Enemy targetEnemy, float bananaSpeed, float bananaDamage)
    {
        this._summonPos = summonPos;
        this._targetPos = targetPos;
        this._targetEnemy = targetEnemy;
        //this._bananaSpeed = bananaSpeed;
        this._bananaDamage = bananaDamage;
        transform.position = summonPos;
        startTime = Time.time;
        _projectileState = ProjectileState.Throwing; 
    }

    private void FixedUpdate()
    {
        transform.Rotate(Vector3.forward * 20f);

       
        if ((_projectileState == ProjectileState.Throwing && Vector3.Distance(transform.position, _targetPos) > 0.1f) ||
            (_projectileState == ProjectileState.Returning && Vector3.Distance(transform.position, _summonPos) > 0.1f))
        {
            ThrowBanana(throwing ? _summonPos : _targetPos, throwing ? _targetPos : _summonPos, throwing ? direction1 : direction2);
            return;
        } 
        else
        {
            _projectileState = ProjectileState.Returning;
            startTime = Time.time;  
            throwing = !throwing;
        }

        if(_projectileState == ProjectileState.Returning && Vector3.Distance(transform.position, _summonPos) <= 0.1f){
            DestroySelf();
        }        
    }

    private void ThrowBanana(Vector3 startPos, Vector3 endPos, Vector3 direction)
    {        
        Vector3 center = (startPos + endPos) * .5f;
        center -= direction;

        Vector3 startRelCenter = startPos - center;
        Vector3 endRelCenter = endPos - center;

        transform.position = Vector3.Slerp(startRelCenter, endRelCenter, Time.time - startTime * 1f) + center;
    }    

    /*
    private void Update()
    {
        if(_targetEnemy != null)
        {
            Vector3 targetPosition = _targetEnemy.transform.position;              
            Vector3 direction = (targetPosition - transform.position).normalized;
            BananaMovementAndRotation(direction, _bananaSpeed);

            if(CheckForCollision(transform.position, targetPosition))
            {
                OnBananaHitEnemy?.Invoke(this);
                _targetEnemy.TakeDamage(_bananaDamage);
                DestroySelf();
            }
        }        
        else
        {
            DestroySelf();
        }        
    }         

    private void BananaMovementAndRotation(Vector3 direction, float movementSpeed)
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
    }*/


    private void DestroySelf()
    {
        DestroyBanana?.Invoke(this);
    }
}

public enum ProjectileState
{
    Throwing = 0,
    Returning = 1
}

