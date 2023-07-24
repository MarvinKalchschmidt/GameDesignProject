using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EnemySpawnData", menuName = "EnemySpawnData")]
public class NewBehaviourScript : ScriptableObject
{
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private int _enemyID;
    [SerializeField] private string _enemyName;
}
