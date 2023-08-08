using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyWaveSpawner : MonoBehaviour
{     
    [SerializeField] private EnemyWave[] _enemyWaves;
    [SerializeField] private float _timeBetweenWaves = 20.0f;
    [SerializeField] private float _countdown;

    public static event Action<int> OnWaveCompleted;
    public static event Action<int> OnWaveStarting;

    public List<Enemy> enemiesAlive = new List<Enemy>();
    private ObjectPool<Enemy> _enemyPool;  
    private int _currentWaveIndex;
    public bool _spawnEnemies = true;


    private void Start()
    {  
        _enemyPool = new ObjectPool<Enemy>(CreateEnemyPoolObject, OnTakeEnemyFromPool, OnReturnEnemyToPool, OnDestroyEnemy, false, 50, 200);       
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

    public void StartEnemySpawning()
    {
        _countdown = _timeBetweenWaves;
        StartCoroutine(RunSpawner());
    }

    private Enemy CreateEnemyPoolObject()
    {
        EnemyWave currentWave = _enemyWaves[_currentWaveIndex];
        Enemy enemy = Instantiate(currentWave.GetEnemyPrefabs[UnityEngine.Random.Range(0, currentWave.GetEnemyPrefabs.Length)]);
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
        yield return new WaitForSeconds(_countdown);
        while(_spawnEnemies && _currentWaveIndex < MaxWaveCount)
        {
            //sends +1 because of zero-based array indexing for UI
            OnWaveStarting?.Invoke(_currentWaveIndex + 1);

            yield return SpawnEnemyWave(_enemyWaves[_currentWaveIndex]);
           
            yield return new WaitWhile(AllEnemiesDefeated);
            _currentWaveIndex++;
            OnWaveCompleted?.Invoke(_currentWaveIndex);

            yield return new WaitForSeconds(_timeBetweenWaves);
        }
    }   

    private IEnumerator SpawnEnemyWave(EnemyWave currentWave)
    {
        Debug.Log($"{currentWave.WaveName} Incoming...");
        for(int i = 0; i < currentWave.AmountOfEnemies; i++)
        {
            _enemyPool.Get();
            yield return new WaitForSeconds(currentWave.SpawnRate);
        }
        _countdown = _timeBetweenWaves;
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