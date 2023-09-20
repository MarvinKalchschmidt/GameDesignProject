using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealFruitSpawner : MonoBehaviour
{     
    [SerializeField] private HealFruit _healFruit;
    [SerializeField] private int _maxFruit;
    [SerializeField] private float _minDelay = 15f;
    [SerializeField] private float _maxDelay = 30f;
    [SerializeField] private BoxCollider2D _boxCollider;
    
    private Bounds _boxColliderBounds;
    public List<HealFruit> _healFruitActive;
    public bool _spawnHealFruit = false;
    public bool _executing = false;


    private void OnEnable()
    {
        HealFruit.OnHealFruitDestroyed += RemoveHealFruit;
    }

    private void OnDisable()
    {
        HealFruit.OnHealFruitDestroyed -= RemoveHealFruit;
    }
    private void Start()
    {
        if(_boxCollider == null)
        {
            _boxCollider = GetComponent<BoxCollider2D>();
        }
        _boxColliderBounds = _boxCollider.bounds;
        _healFruitActive = new List<HealFruit>();

        StartHealFruitSpawning();
    }

    private void Update()
    {
        if (!_spawnHealFruit)
        {
            return;
        }

        if (_healFruitActive.Count < MaxFruitCount && !_executing)
        {
            StartCoroutine(Test());
        }
    }   

    public int MaxFruitCount
    {
        get => _maxFruit;
    }

    public void StartHealFruitSpawning()
    {
        _spawnHealFruit = true;
    }   

    private IEnumerator Test()
    {
        _executing = true;
        float randomDelay = Random.Range(_minDelay, _maxDelay);

        yield return new WaitForSeconds(randomDelay);

        SpawnHealFruit(GetLocationFromSpawnArea());
        _executing = false;
    }

    private Vector3 GetLocationFromSpawnArea()
    {
        float randomX = Random.Range(_boxColliderBounds.min.x, _boxColliderBounds.max.x);
        float randomY = Random.Range(_boxColliderBounds.min.y, _boxColliderBounds.max.y);
         
        return new Vector3(randomX, randomY, 2);
    }

    private HealFruit SpawnHealFruit(Vector3 location)
    {
        HealFruit healFruit = Instantiate(_healFruit, location, Quaternion.identity, transform);
        _healFruitActive.Add(healFruit);
        return healFruit;
    }

    private void RemoveHealFruit(HealFruit healFruit)
    {
        _healFruitActive.Remove(healFruit);
    }
}