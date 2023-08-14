using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public abstract class Tower : MonoBehaviour
{ 
    [SerializeField] protected TowerType _towerType;
    [SerializeField] protected float _range;
    [SerializeField] protected float _damage;
    [SerializeField] protected float _attackSpeed;
    [SerializeField] protected float _cooldown;
    [SerializeField] protected Transform _projectileSummonPoint;
    [SerializeField] protected CircleCollider2D _circleCollider;
    [SerializeField] protected SpriteRenderer _rangeRadiusSpriteRenderer;
    [SerializeField] protected bool _towerIsPlaced;
    [SerializeField] protected List<Enemy> _enemiesWithinRange;

    public TowerType TowerType { get => _towerType; set => _towerType = value; }    
    public float Damage { get => _damage; set => _damage = value; }    
    public float AttackSpeed { get => _attackSpeed; set => _attackSpeed = value; }    
    public bool TowerIsPlaced { get => _towerIsPlaced;  set => _towerIsPlaced = value; }


    private void Awake()
    {
        _enemiesWithinRange = new List<Enemy>();
        if(_circleCollider == null)
        {
            _circleCollider = GetComponent<CircleCollider2D>();
        }
        if (_rangeRadiusSpriteRenderer == null)
        {
            _rangeRadiusSpriteRenderer = _projectileSummonPoint.GetComponentInChildren<SpriteRenderer>();
        }
    }  


    protected abstract void Attack();       

    //Would've used this for Upgrades, but those are not implemented yet.
    protected void UpdateTowerRange(float addedRange)
    {       
        if(_circleCollider != null)
        {           
            _circleCollider.radius += addedRange * 0.1f;
        }
    }

    public virtual void ShowRangeRadius()
    {
        _rangeRadiusSpriteRenderer.enabled = true;
    }

    public virtual void HideRangeRadius()
    {
        _rangeRadiusSpriteRenderer.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("EnemyTrigger");
        if (other.CompareTag("Enemy"))
        {
            _enemiesWithinRange.Add(other.gameObject.GetComponent<Enemy>());
        }      
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            _enemiesWithinRange.Remove(other.gameObject.GetComponent<Enemy>());
        }
    }

    private void OnMouseDown()
    {
        if(_towerIsPlaced)
        {
            //ShowRangeRadius();
        }
    }

    private void OnMouseExit()
    {
        //HideRangeRadius();
    }
}

public enum TowerType
{
    Monkey = 0,
    Toucan = 1,
    Leopard = 2
}
