using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyWaveSpawner : MonoBehaviour
{     
    [SerializeField] private EnemyWave[] _enemyWaves;
    [SerializeField] private float _initialCountdown;
    [SerializeField] private float _timeBetweenWaves = 5.0f;
    [SerializeField] private int _currentWaveIndex;

    public static event Action<int> OnWaveCompleted;
    public static event Action<int> OnWaveStarting;
    public static event Action<string, float> OnDisplayCountdown;

    public List<Enemy> enemiesAlive = new List<Enemy>();
    //private ObjectPool<Enemy> _enemyPool;
    private ObjectPool<Enemy> _enemyPoolDuo;
    private ObjectPool<Enemy> _enemyPoolChainsaw;
    private ObjectPool<Enemy> _enemyPoolExcavator;
    public bool _spawnEnemies = true;
    public int _enemyPrefabListIndex;


    private void Start()
    {  
        //_enemyPool = new ObjectPool<Enemy>(CreateEnemyPoolObject, OnTakeEnemyFromPool, OnReturnEnemyToPool, OnDestroyEnemy, false, 50, 500);       
        _enemyPoolDuo = new ObjectPool<Enemy>(CreateEnemyPoolObject, OnTakeEnemyFromPool, OnReturnEnemyToPool, OnDestroyEnemy, false, 50, 100);       
        _enemyPoolChainsaw = new ObjectPool<Enemy>(CreateEnemyPoolObject, OnTakeEnemyFromPool, OnReturnEnemyToPool, OnDestroyEnemy, false, 50, 100);       
        _enemyPoolExcavator = new ObjectPool<Enemy>(CreateEnemyPoolObject, OnTakeEnemyFromPool, OnReturnEnemyToPool, OnDestroyEnemy, false, 50, 100);       
    }

    public int WaveIndex
    {
        get => _currentWaveIndex;
        set => _currentWaveIndex = value;
    }

    public int MaxWaveCount
    {
        get => _enemyWaves.Length;       
    }

    public void StartTutorialEnemySpawning()
    {
        StartCoroutine(RunTutorialWave());
    }

    public void StartEnemySpawning()
    {
        StartCoroutine(RunSpawner());
    }

    private Enemy CreateEnemyPoolObject()
    {
        EnemyWave currentWave = _enemyWaves[_currentWaveIndex];
        Enemy[] enemyPrefabs = currentWave.GetEnemyPrefabs;
        Enemy enemy = Instantiate(enemyPrefabs[_enemyPrefabListIndex]);
        enemy.DestroyEnemy += ReleaseEnemyToPool;
        enemy.gameObject.SetActive(false);
        return enemy;
    }

    private void OnTakeEnemyFromPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(true);
        InitEnemy(enemy);
        enemiesAlive.Add(enemy);               
    }

    private void OnReturnEnemyToPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
    }    

    private void ReleaseEnemyToPool(Enemy enemy)
    {
        enemiesAlive.Remove(enemy);
        //_enemyPool.Release(enemy);
        switch (enemy.EnemyType)
        {
            case EnemyType.Duo: _enemyPoolDuo.Release(enemy); break;
            case EnemyType.Chainsaw: _enemyPoolChainsaw.Release(enemy); break;
            case EnemyType.Excavator: _enemyPoolExcavator.Release(enemy); break;
        }
    }

    private void OnDestroyEnemy(Enemy enemy)
    {
        enemiesAlive.Remove(enemy);
        Destroy(enemy.gameObject);
    }

    private IEnumerator RunSpawner()
    {   
        yield return Countdown("", _initialCountdown);
        
        while (_spawnEnemies && _currentWaveIndex < MaxWaveCount)
        {
            //used to send +1 because of zero-based array indexing for UI
            OnWaveStarting?.Invoke(_currentWaveIndex);

            yield return SpawnEnemyWave(_enemyWaves[_currentWaveIndex]);
           
            yield return new WaitWhile(AllEnemiesDefeated);
            _currentWaveIndex++;
            OnWaveCompleted?.Invoke(_currentWaveIndex);

            if(_currentWaveIndex < MaxWaveCount)
            {
                yield return Countdown(_enemyWaves[_currentWaveIndex].WaveName, _timeBetweenWaves);
            }
        }
    }

    private IEnumerator RunTutorialWave()
    {
        if (_spawnEnemies)
        {
            OnWaveStarting?.Invoke(_currentWaveIndex);

            yield return SpawnEnemyWave(_enemyWaves[_currentWaveIndex]);

            yield return new WaitWhile(AllEnemiesDefeated);
            _currentWaveIndex++;
        }
    }

    private IEnumerator Countdown(string waveName, float countdowntime)
    {
        Debug.Log("Starting Countdown");        
        OnDisplayCountdown.Invoke(waveName, countdowntime);
        //+ 1.5f to match UI Coutdown Time
        yield return new WaitForSeconds(countdowntime + 1.5f);
    }    

    private IEnumerator SpawnEnemyWave(EnemyWave currentWave)
    {
        _enemyPrefabListIndex = 0;
        Debug.Log($"{currentWave.WaveName} Incoming...");
        for(int i = 0; i < currentWave.GetEnemyPrefabs.Length; i++)
        {
            //_enemyPool.Get(); switch (enemy.EnemyType)
            switch (currentWave.GetEnemyPrefabs[i].EnemyType)
            {
                case EnemyType.Duo: _enemyPoolDuo.Get(); break;
                case EnemyType.Chainsaw: _enemyPoolChainsaw.Get(); break;
                case EnemyType.Excavator: _enemyPoolExcavator.Get(); break;
            }
            _enemyPrefabListIndex++;
            yield return new WaitForSeconds(currentWave.SpawnRate);
        }
    }

    private void InitEnemy(Enemy enemy)
    {
        enemy.transform.position = transform.position;
        enemy.transform.rotation = Quaternion.identity;
        enemy.transform.SetParent(transform);
    }  

    private bool AllEnemiesDefeated()
    {
        foreach (Enemy enemy in enemiesAlive)
        {
            if (enemy == null)
            {
                enemiesAlive.Remove(enemy);
            }
        }
        return enemiesAlive.Count > 0;
    }
}