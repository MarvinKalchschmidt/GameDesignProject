using UnityEngine;

[CreateAssetMenu(fileName = "New EnemyWave", menuName = "EnemyWave")]
public class EnemyWave : ScriptableObject
{
    [SerializeField] private string _waveName;
    [SerializeField] private float _spawnRate;
    //[SerializeField] private int _amountOfEnemies;
    [SerializeField] private Enemy[] _enemyPrefabs;
    [SerializeField] private int _waveClearReward;

    public string WaveName { get => _waveName; set => _waveName = value; }
    public float SpawnRate { get => _spawnRate; set => _spawnRate = value; }
    //public int AmountOfEnemies { get => _amountOfEnemies; set => _amountOfEnemies = value; }
    public Enemy[] GetEnemyPrefabs { get => _enemyPrefabs; }
    public int WaveClearReward { get => _waveClearReward; set => _waveClearReward = value; }
}
