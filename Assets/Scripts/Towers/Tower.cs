using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CircleCollider2D))]
public abstract class Tower : MonoBehaviour
{ 
    [SerializeField] protected TowerType _towerType;
    [SerializeField] protected float _range;
    [SerializeField] protected float _damage;
    [SerializeField] protected float _attackSpeed;
    [SerializeField] protected float _cooldown;
    [SerializeField] protected Vector3 _towerLocation;
    [SerializeField] protected CircleCollider2D _circleCollider;
    [SerializeField] protected bool _towerIsPlaced;
    //[SerializeField] protected List<Enemy> _enemiesWithinRange;
    [SerializeField] protected Animator _animator;
    [SerializeField] protected TargetDetectionUtil _targetDetection;
    [SerializeField] protected TowerUpgradeHolder _towerUpgradeHolder;
    protected bool toggleUpgradeButton = false;

    private bool _damageUpgrade = false;
    private bool _speedUpgrade = false;

    public static event Action<int> OnUpgradeBought;
    public static event Action<string> OnDisplayMessage;
    public static event Action OnClickTower;

    public TowerType TowerType { get => _towerType; set => _towerType = value; }    
    public float Damage { get => _damage; set => _damage = value; }    
    public float AttackSpeed { get => _attackSpeed; set => _attackSpeed = value; }    
    public float Cooldown { get => _cooldown; set => _cooldown = value; }    
    public Vector3 TowerStartLocation { get => _towerLocation; set => _towerLocation = value; }    
    public bool TowerIsPlaced { get => _towerIsPlaced;  set => _towerIsPlaced = value; }
    public bool DamageUpgrade { get => _damageUpgrade;  set => _damageUpgrade = value; }
    public bool SpeedUpgrade { get => _speedUpgrade;  set => _speedUpgrade = value; }


    private void Awake()
    {
        //_enemiesWithinRange = new List<Enemy>();
        if(_circleCollider == null)
        {
            _circleCollider = GetComponent<CircleCollider2D>();
        }
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
        }
        if(_targetDetection == null)
        {
            _targetDetection = GetComponentInChildren<TargetDetectionUtil>();
        }
        if (_towerUpgradeHolder == null)
        {
            _towerUpgradeHolder = GameObject.FindGameObjectWithTag("TowerUpgradeHolder").GetComponent<TowerUpgradeHolder>();
        }
    } 
    
    public void InitTower(Vector3 towerPosition, float damage)
    {
        TowerStartLocation = towerPosition;
        transform.position = towerPosition;
        _damage = damage;
        _towerIsPlaced = true;
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

    protected void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (_towerIsPlaced)
        {
            OnClickTower?.Invoke();
            if (!toggleUpgradeButton)
            {
                //ShowRangeRadius();
                Debug.Log("Click on Tower");
                _towerUpgradeHolder.ShowUpgradesForTower(this, transform.position);
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

    public void UpgradeTower(TowerUpgrade towerUpgrade)
    {
        if (towerUpgrade.UpgradeType == UpgradeType.Damage && !DamageUpgrade)
        {
            DamageUpgrade = true;
            Damage += towerUpgrade.UpgradeAmount;
            OnUpgradeBought?.Invoke(towerUpgrade.Cost * -1);
        }
        else if (towerUpgrade.UpgradeType == UpgradeType.Speed && !SpeedUpgrade)
        {
            SpeedUpgrade = true;
            AttackSpeed += towerUpgrade.UpgradeAmount;
            OnUpgradeBought?.Invoke(towerUpgrade.Cost * -1);
        } else
        {
            OnDisplayMessage($"Can't buy anymore {towerUpgrade.UpgradeType} Upgrades for this Tower");
        }
    }
}

public enum TowerType
{
    Monkey = 0,
    Toucan = 1,
    Leopard = 2
}
