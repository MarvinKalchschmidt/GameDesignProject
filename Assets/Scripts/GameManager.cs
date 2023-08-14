using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private EnemyWaveSpawner _enemyWaveSpawner;
    [SerializeField] private TreeOfLife _treeOFLife;
    [SerializeField] private PolygonCollider2D _bounds;

    private void Start() {
        _enemyWaveSpawner.StartEnemySpawning();
    }
}
