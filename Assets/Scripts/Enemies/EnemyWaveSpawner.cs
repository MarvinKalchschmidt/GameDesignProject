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

    public static event Action<int> OnWaveCompleted;
    public static event Action<int> OnWaveStarting;
    public static event Action<float> OnDisplayCountdown;

    public List<Enemy> enemiesAlive = new List<Enemy>();
    private ObjectPool<Enemy> _enemyPool;  
    private int _currentWaveIndex;
    public bool _spawnEnemies = true;


    private void Start()
    {  
        _enemyPool = new ObjectPool<Enemy>(CreateEnemyPoolObject, OnTakeEnemyFromPool, OnReturnEnemyToPool, OnDestroyEnemy, false, 50, 500);       
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
        Enemy enemy = Instantiate(enemyPrefabs[UnityEngine.Random.Range(0, enemyPrefabs.Length)]);
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

    private void OnDestroyEnemy(Enemy enemy)
    {
        enemiesAlive.Remove(enemy);
        Destroy(enemy.gameObject);
    }

    private void ReleaseEnemyToPool(Enemy enemy)
    {
        enemiesAlive.Remove(enemy);
        _enemyPool.Release(enemy);
    }

    private IEnumerator RunSpawner()
    {   
        yield return Countdown(_initialCountdown);
        
        while (_spawnEnemies && _currentWaveIndex < MaxWaveCount)
        {
            //used to send +1 because of zero-based array indexing for UI
            OnWaveStarting?.Invoke(_currentWaveIndex);

            yield return SpawnEnemyWave(_enemyWaves[_currentWaveIndex]);
           
            yield return new WaitWhile(AllEnemiesDefeated);
            _currentWaveIndex++;
            OnWaveCompleted?.Invoke(_currentWaveIndex);

            yield return Countdown(_timeBetweenWaves);
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

    private IEnumerator Countdown(float countdowntime)
    {
        Debug.Log("Starting Countdown");        
        OnDisplayCountdown.Invoke(countdowntime);
        //+ 1.5f to match UI Coutdown Time
        yield return new WaitForSeconds(countdowntime + 1.5f);
    }    

    private IEnumerator SpawnEnemyWave(EnemyWave currentWave)
    {
        Debug.Log($"{currentWave.WaveName} Incoming...");
        for(int i = 0; i < currentWave.AmountOfEnemies; i++)
        {
            _enemyPool.Get();
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