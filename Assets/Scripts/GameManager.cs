using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private EnemyWaveSpawner _enemyWaveSpawner;
    [SerializeField] private TreeOfLife _treeOFLife;

    private void Start() {
        _enemyWaveSpawner.StartEnemySpawning();
    }


}
