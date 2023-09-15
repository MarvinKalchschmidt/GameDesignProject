using UnityEngine;

public class TutorialManager : Singleton<TutorialManager>
{
    [SerializeField] GameObject[] _popups;

    private int _popUpIndex;

    private void OnEnable()
    {
        //Events ;
        //Enemy.OnEnemyDiesFromDamage += AddKillReward;
        //EnemyWaveSpawner.OnWaveStarting += ManageWaveCount;
    }

    private void OnDisable()
    {
        //Enemy.OnEnemyDiesFromDamage -= AddKillReward;
        //EnemyWaveSpawner.OnWaveStarting -= ManageWaveCount;
    }

    private void StartTutorial()
    {

    }

    private void Update()
    {
        if(_popUpIndex == 0)
        {
     
        }
    }
}
