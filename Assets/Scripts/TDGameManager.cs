using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TDGameManager : Singleton<TDGameManager>
{
    /*
    [SerializeField] private LevelGenerator _levelGenerator;
    [SerializeField] private EnemyWaveSpawner _enemyWaveSpawner;
    [SerializeField] private TowerBuildManager _towerBuildManager;
    [SerializeField] private GameObject _towerDefenseMenuUI;
    [SerializeField] private GameObject _towerDefenseGameUI;
    [SerializeField] private Transform _camera;
    [SerializeField] private TMP_Text _livesCountText;
    [SerializeField] private TMP_Text _moneyCountText;
    [SerializeField] private TMP_Text _wavesCountText;
    [SerializeField] private GameObject _displayMessage;
    [SerializeField] private int _money;
    [SerializeField] private int _lives;
    [SerializeField] private int _waveCount;
    [SerializeField] private int _startMoney;
    [SerializeField] private int _startLives;

    private Grid<Tile> _grid;
    private List<WayPoint> _wayPointsList;
    private Tile _startTile;
    private bool _loadGameSave = false;
    private AsyncOperation _asyncOperation;

    public TowerBuildManager BuildManager { get => _towerBuildManager; set => _towerBuildManager = value; }
    public Tile StartTile { get => _startTile; set => _startTile = value; }
    public bool LoadGameSave { get => _loadGameSave; set => _loadGameSave = value; }
    public List<WayPoint> GetEnemyWayPoints { get => _wayPointsList; set => _wayPointsList = value; }
    public int Money { get => _money; set { _money = value; UpdateMoneyCount(); } } 
    public int Lives { get => _lives; set { _lives = value; UpdateLivesCount(); } }   
    public int  WaveCount { get => _waveCount; set { _waveCount = value; _enemyWaveSpawner.WaveIndex = value;  UpdateWaveCount(_waveCount, 5); } }   
    public Grid<Tile> Grid { get => _grid; set => _grid = value; }

    public void Init(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "TowerDefense")
        {
            _towerDefenseMenuUI.SetActive(false);
            _towerDefenseGameUI.SetActive(true);
            _levelGenerator = GameObject.Find("LevelGenerator").GetComponent<LevelGenerator>();
            _enemyWaveSpawner = GameObject.Find("EnemyWaveSpawner").GetComponent<EnemyWaveSpawner>();
            _towerBuildManager = GameObject.Find("TowerBuildManager").GetComponent<TowerBuildManager>();
            _grid = _levelGenerator.GenerateMapGrid();
            _startTile = _levelGenerator.StartTile;
            _wayPointsList = _levelGenerator.GetEnemyWayPoints;
            _enemyWaveSpawner.StartEnemySpawning();
            SetCamera();
            _money = _startMoney;
            _lives = _startLives;
            UpdateMoneyCount();
            UpdateLivesCount();
            UpdateWaveCount(_enemyWaveSpawner.WaveIndex, _enemyWaveSpawner.MaxWaveCount);

            if (_loadGameSave)
            {
                SaveLoadManager.Instance.LoadTowerDefenseData();
                _loadGameSave = false;
            }
        }
    }

    private void OnEnable()
    {
        Enemy.OnEnemyDiesFromDamage += AddKillReward;
        Enemy.OnEnemyReachFinalWayPoint += TakeEnemyDamage;
        EnemyWaveSpawner.OnWaveStarting += ManageWaveCount;
        EnemyWaveSpawner.OnWaveCompleted += CheckForLastWaveCompleted;
        TowerBuildManager.OnTowerPlaced += ManageMoney;
        TowerBuildManager.OnDisplayMessage += DisplayMessage;
        SaveSystem.OnDisplayMessage += DisplayMessage;
        SceneManager.sceneLoaded += Init;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyDiesFromDamage -= AddKillReward;
        Enemy.OnEnemyReachFinalWayPoint -= TakeEnemyDamage;
        EnemyWaveSpawner.OnWaveStarting -= ManageWaveCount;
        EnemyWaveSpawner.OnWaveCompleted -= CheckForLastWaveCompleted;
        TowerBuildManager.OnTowerPlaced -= ManageMoney;
        TowerBuildManager.OnDisplayMessage -= DisplayMessage;
        SaveSystem.OnDisplayMessage -= DisplayMessage;
        SceneManager.sceneLoaded -= Init;
    }       

    private void AddKillReward(int reward)
    {
        ManageMoney(reward);
    }

    private void ManageMoney(int moneyToManage)
    {
        _money += moneyToManage;
        UpdateMoneyCount();
    }

    private void TakeEnemyDamage(int damage)
    {
        _lives -= damage;
        UpdateLivesCount();       
    }

    public void ManageWaveCount(int currentWaveIndex)
    {
        _waveCount = currentWaveIndex;
        UpdateWaveCount(currentWaveIndex, _enemyWaveSpawner.MaxWaveCount);
    }
    
    private void UpdateMoneyCount()
    {
        _moneyCountText.SetText($"Money: {_money}");
    }

    private void UpdateLivesCount()
    {
        _livesCountText.SetText($"Lives: {_lives}");
        if (_lives == _startLives / 10)
        {
            PreloadMainMenu();
        }
        if (_lives <= 0)
        {
            GameOver();
        }
    }

    private void UpdateWaveCount(int currentWaveIndex, int maxWaves)
    {
        _wavesCountText.SetText($"Wave: {currentWaveIndex}/{maxWaves}");
        if (currentWaveIndex == maxWaves)
        {
            PreloadMainMenu();
        }       
    }

    private void CheckForLastWaveCompleted(int currentWaveIndex)
    {
        if(currentWaveIndex == _enemyWaveSpawner.MaxWaveCount)
        {
            GameOver();
        }
    }

    private void DisplayMessage(string messageToDisplay)
    {
        _displayMessage.GetComponentInChildren<TMP_Text>().text = messageToDisplay;
        _displayMessage.GetComponent<FadeComponent>().Fade();
    }

    private void SetCamera()
    {
        _camera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        _camera.transform.position = new Vector3((_grid.Width / 2), (_grid.Height / 2), -10);        
    }

    private void PreloadMainMenu()
    {
        _asyncOperation = SceneManager.LoadSceneAsync("TowerDefenseMenu");
        _asyncOperation.allowSceneActivation = false;
    }

    private void GameOver()
    {        
        Debug.Log("GameOver");
        _towerDefenseMenuUI.SetActive(true);
        _towerDefenseGameUI.SetActive(false);        
        //ResetGame();
        _asyncOperation.allowSceneActivation = true;
    }*/
}