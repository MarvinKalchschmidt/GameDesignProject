using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Tower : MonoBehaviour
{ 
    [SerializeField] private TowerType _towerType;
    [SerializeField] private float _range;
    [SerializeField] private float _damage;
    [SerializeField] private float _cooldown;
    [SerializeField] private Transform _projectileSummonPoint;
    [SerializeField] private CircleCollider2D _circleCollider;
    [SerializeField] private SpriteRenderer _rangeRadiusSpriteRenderer;

    private float _cooldownTimer;    
    public bool _towerIsPlaced;
    private int _cost;
    private List<Vector2Int> _buildingBounds;  
    private List<Enemy> _enemiesWithinRange;

    public TowerType TowerType { get => _towerType; set => _towerType = value; }
    public List<Vector2Int> BuildingBounds { get => _buildingBounds;  set => _buildingBounds = value; }   
    public bool TowerIsPlaced { get => _towerIsPlaced;  set => _towerIsPlaced = value; }
    public int Cost { get => _cost;  set => _cost = value; }


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

    private void Update()
    {
        if (_towerIsPlaced && _enemiesWithinRange.Count > 0)
        { 
            //Maybe add GameState to only check when Wave is generating
            _cooldownTimer -= Time.deltaTime;
            
            if (_cooldownTimer <= 0f)
            {
                Enemy targetEnemy = GetClosestEnemy();
                _cooldownTimer = _cooldown;                
                ShootArrow(_projectileSummonPoint.transform.position, targetEnemy, 4f, _damage);
            }
        }        
    }
    
    private Enemy GetClosestEnemy()
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

    private void ShootArrow(Vector3 arrowSummPosition, Enemy targetEnemy, float speed, float damage)
    {
        ProjectileSpawner.Instance.SummonArrow(arrowSummPosition, targetEnemy, speed, damage);       
    }    

    //Would've used this for Upgrades, but those are not implemented yet.
    private void UpdateTowerRange(float addedRange)
    {       
        if(_circleCollider != null)
        {           
            _circleCollider.radius += addedRange * 0.1f;
        }
    }

    public void ShowRangeRadius()
    {
        _rangeRadiusSpriteRenderer.enabled = true;
    }

    public void HideRangeRadius()
    {
        _rangeRadiusSpriteRenderer.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
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
            ShowRangeRadius();
        }
    }

    private void OnMouseExit()
    {
        HideRangeRadius();
    }
}

public enum TowerType
{
    Monkey = 0,
    Toucan = 1,
    Puma = 2
}
