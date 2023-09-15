using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private EnemyWaveSpawner _enemyWaveSpawner;
    [SerializeField] private TowerPlacementManager _towerPlacementManager;
    [SerializeField] private TreeOfLife _treeOFLife;
    [SerializeField] private Camera _camera;


    /**Game Logic**/
    [SerializeField] private int _money;
    [SerializeField] private int _waveCount;
    [SerializeField] private int _startMoney;
    private static GameOverState _gameOverState;

    /**UI**/
    [SerializeField] private TMP_Text _moneyCountText;
    [SerializeField] private TMP_Text _wavesCountText;
    [SerializeField] private GameObject _displayMessage;

    private AsyncOperation _asyncOperation;

    public int Money { get => _money; set { _money = value; UpdateMoneyCount(); } }
    public int WaveCount { get => _waveCount; set { _waveCount = value; _enemyWaveSpawner.WaveIndex = value; UpdateWaveCount(_waveCount, 5); } }



    private void Init(Scene scene, LoadSceneMode mode) {
        Debug.Log("Init Game");
        if (scene.name == "GameScene")
        {
            if (_enemyWaveSpawner == null)
            {
                _enemyWaveSpawner = GameObject.Find("EnemyWaveSpawner").GetComponent<EnemyWaveSpawner>();
            }

            if (_enemyWaveSpawner == null)
            {
                _towerPlacementManager = GameObject.Find("TowerPlacementManager").GetComponent<TowerPlacementManager>();
            }
            _money = _startMoney;
            _enemyWaveSpawner.StartEnemySpawning();
            UpdateMoneyCount();
            UpdateWaveCount(_enemyWaveSpawner.WaveIndex, _enemyWaveSpawner.MaxWaveCount);
        }
    }

    private void OnEnable()
    {
        Enemy.OnEnemyDiesFromDamage += AddKillReward;
        EnemyWaveSpawner.OnWaveStarting += ManageWaveCount;
        EnemyWaveSpawner.OnWaveCompleted += CheckForLastWaveCompleted;
        TreeOfLife.OnDamageTaken += CheckForTreeAboutToDie;
        TreeOfLife.OnTreeDead += GameOver;
        TowerPlacementManager.OnTowerPlaced += ManageMoney;
        TowerPlacementManager.OnDisplayMessage += DisplayMessage;
        TowerButton.OnDisplayMessage += DisplayMessage;      
        UpgradeButton.OnDisplayMessage += DisplayMessage;      
        SceneManager.sceneLoaded += Init;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyDiesFromDamage -= AddKillReward;
        EnemyWaveSpawner.OnWaveStarting -= ManageWaveCount;
        EnemyWaveSpawner.OnWaveCompleted -= CheckForLastWaveCompleted;
        TreeOfLife.OnDamageTaken -= CheckForTreeAboutToDie;
        TreeOfLife.OnTreeDead -= GameOver;
        TowerPlacementManager.OnTowerPlaced -= ManageMoney;
        TowerPlacementManager.OnDisplayMessage -= DisplayMessage;
        TowerButton.OnDisplayMessage -= DisplayMessage;
        UpgradeButton.OnDisplayMessage -= DisplayMessage;
        SceneManager.sceneLoaded -= Init;
    }

    private void DisplayMessage(string messageToDisplay)
    {
        _displayMessage.GetComponentInChildren<TMP_Text>().text = messageToDisplay;
        _displayMessage.GetComponent<FadeComponent>().Fade();
    }

    private void AddKillReward(int reward)
    {
        ManageMoney(reward);
    }

    private void ManageMoney(int moneyToManage)
    {
        _money = Math.Max(0, _money + moneyToManage);
        UpdateMoneyCount();
    }
    
    private void UpdateMoneyCount()
    {
        _moneyCountText.SetText($"{_money}");
    }   

    public void ManageWaveCount(int currentWaveIndex)
    {
        _waveCount = currentWaveIndex;
        UpdateWaveCount(currentWaveIndex, _enemyWaveSpawner.MaxWaveCount);
    }

    private void UpdateWaveCount(int currentWaveIndex, int maxWaves)
    {
        _wavesCountText.SetText($"Wave: {currentWaveIndex}/{maxWaves}");
        if (currentWaveIndex == maxWaves)
        {
            //PreloadMainMenu();
        }
    }

    private void CheckForTreeAboutToDie(float currentHealth)
    {
        if(currentHealth <= _treeOFLife.MaxHealth * 0.2f)
        {

        }
    }

    private void CheckForLastWaveCompleted(int currentWaveIndex)
    {
        if (currentWaveIndex == _enemyWaveSpawner.MaxWaveCount)
        {
            GameOver(GameOverState.Win);
        }
    }  

    private void GameOver(GameOverState gameOverState)
    {
        _gameOverState = gameOverState;
        if (gameOverState == GameOverState.Win)
        {
            Debug.Log("YOU WIN");
            
        }
        else
        {
            Debug.Log("YOU LOSE");
            DisplayMessage("YOU LOSE");
        }
        //Time.timeScale = 0; Freeze game
        //_towerDefenseMenuUI.SetActive(true);
        //_towerDefenseGameUI.SetActive(false);
        //ResetGame();
        //_asyncOperation.allowSceneActivation = true;
    }
}

public enum GameOverState
{
    Win = 0,
    Loss = 1
}