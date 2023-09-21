using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private EnemyWaveSpawner _enemyWaveSpawner;
    [SerializeField] private TowerPlacementManager _towerPlacementManager;
    [SerializeField] private TutorialManager _tutorialManager;
    [SerializeField] private TreeOfLife _treeOFLife;
    [SerializeField] private Camera _camera;
    [SerializeField] private CameraMovement _cameraMovement;
    [SerializeField] private bool _controllsEnabled;    
    
    /**Game Logic**/
    [SerializeField] private int _money;
    [SerializeField] private int _waveCount;
    [SerializeField] private int _startMoney;
    [SerializeField] private static bool _skipTutorial;
    [SerializeField] private static GameOverState _gameOverState;

    /**UI**/
    [SerializeField] private TMP_Text _moneyCountText;
    [SerializeField] private TMP_Text _wavesCountText;
    [SerializeField] private GameObject _displayMessage;
    [SerializeField] private GameObject _countdownMessage;

    private AsyncOperation _asyncOperation;

    public int Money { get => _money; set { _money = value; UpdateMoneyCount(); } }
    public int WaveCount { get => _waveCount; set { _waveCount = value; UpdateWaveCount(_waveCount, _enemyWaveSpawner.MaxWaveCount); } }
    public bool ControllsEnabled { get => _controllsEnabled; set { _controllsEnabled = value; } }
    public static bool SkipTutorial  { get => _skipTutorial; set { _skipTutorial = value; } }
    public static GameOverState GameOverState { get => _gameOverState; set { _gameOverState = value; } }


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
            UpdateMoneyCount();

            if (_skipTutorial)
            {
                SkipTutorialFunction();
            }           
        }
    }

    private void OnEnable()
    {
        Enemy.OnEnemyDiesFromDamage += AddKillReward;
        EnemyWaveSpawner.OnWaveStarting += ManageWaveCount;
        EnemyWaveSpawner.OnWaveCompleted += CheckForLastWaveCompleted;
        EnemyWaveSpawner.OnDisplayCountdown += WaveCountdown;
        TreeOfLife.OnFirstDamageTaken += StartHealFruitSpawner;
        TreeOfLife.OnTreeHealthLow += CheckForTreeAboutToDie;
        TreeOfLife.OnTreeDead += GameOver;
        TowerPlacementManager.OnTowerPlaced += ManageMoney;
        TowerPlacementManager.OnDisplayMessage += DisplayMessage;
        TowerButton.OnDisplayMessage += DisplayMessage;      
        Tower.OnUpgradeBought += ManageMoney;
        Tower.OnDisplayMessage += DisplayMessage;
        UpgradeButton.OnDisplayMessage += DisplayMessage;
        HealFruit.OnHealFruitClicked += HealTreeOfLife;
        CameraMovement.OnAnimationFinished += EnableControlls;
        TutorialManager.OnTutorialStart += StartTutorial;
        TutorialManager.OnTutorialCompleted += StartWaveSpawner;
        SceneManager.sceneLoaded += Init;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyDiesFromDamage -= AddKillReward;
        EnemyWaveSpawner.OnWaveStarting -= ManageWaveCount;
        EnemyWaveSpawner.OnWaveCompleted -= CheckForLastWaveCompleted;
        EnemyWaveSpawner.OnDisplayCountdown -= WaveCountdown;
        TreeOfLife.OnFirstDamageTaken -= StartHealFruitSpawner;
        TreeOfLife.OnTreeHealthLow -= CheckForTreeAboutToDie;
        TreeOfLife.OnTreeDead -= GameOver;
        TowerPlacementManager.OnTowerPlaced -= ManageMoney;
        TowerPlacementManager.OnDisplayMessage -= DisplayMessage;
        TowerButton.OnDisplayMessage -= DisplayMessage;
        Tower.OnUpgradeBought -= ManageMoney;
        Tower.OnDisplayMessage -= DisplayMessage;
        UpgradeButton.OnDisplayMessage -= DisplayMessage;
        HealFruit.OnHealFruitClicked -= HealTreeOfLife;
        CameraMovement.OnAnimationFinished -= EnableControlls;
        TutorialManager.OnTutorialStart -= StartTutorial;
        TutorialManager.OnTutorialCompleted -= StartWaveSpawner;
        SceneManager.sceneLoaded -= Init;
    }     
    
    private void EnableControlls()
    {
        ControllsEnabled = true;
    }

    private void StartTutorial()
    {
        _enemyWaveSpawner.StartTutorialEnemySpawning();
        UpdateWaveCount(_enemyWaveSpawner.WaveIndex, _enemyWaveSpawner.MaxWaveCount);
    }

    private void SkipTutorialFunction()
    {
        //Skip first Wave
        _enemyWaveSpawner.WaveIndex = 1;
        _cameraMovement.AnimationFinished();
        ControllsEnabled = true;
        StartWaveSpawner();
    }

    private void StartWaveSpawner()
    {
        _tutorialManager.gameObject.SetActive(false);
        _enemyWaveSpawner.StartEnemySpawning();
        UpdateWaveCount(_enemyWaveSpawner.WaveIndex, _enemyWaveSpawner.MaxWaveCount);
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
        _wavesCountText.SetText($"Wave: {currentWaveIndex}/{maxWaves-1}");
        if (currentWaveIndex == maxWaves)
        {
            PreloadEndScene();
        }
    }

    private void CheckForTreeAboutToDie()
    {
        PreloadEndScene();
    }

    private void PreloadEndScene()
    {
        _asyncOperation = SceneManager.LoadSceneAsync("EndScene");
        _asyncOperation.allowSceneActivation = false;
    }

    private void DisplayMessage(string messageToDisplay)
    {
        _displayMessage.GetComponentInChildren<TMP_Text>().text = messageToDisplay;
        _displayMessage.GetComponent<FadeComponent>().Fade();
    }

    private void WaveCountdown(string waveName, float countdown)
    {
        StartCoroutine(StartCountdown(waveName, countdown));
    }

    private IEnumerator StartCountdown(string waveName, float countdown)
    {
        float timeRemaining = countdown;

        _countdownMessage.GetComponentInChildren<TMP_Text>().text = $"{waveName} Starting...";
        _countdownMessage.GetComponent<FadeComponent>().FadeIn();

        yield return new WaitForSeconds(1.5f); // Wait for 1.5 seconds

        while (timeRemaining > 0)
        {
            _countdownMessage.GetComponentInChildren<TMP_Text>().text = Mathf.CeilToInt(timeRemaining).ToString();
            yield return new WaitForSeconds(1.0f); // Wait for 1 second
            timeRemaining -= 1.0f;
        }

        _countdownMessage.GetComponent<FadeComponent>().FadeOut();
    }

    private void HealTreeOfLife(float heal)
    {
        _treeOFLife.Heal(heal);
    }

    private void StartHealFruitSpawner()
    {
        //TODO SPAWNER EINBAUEN
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
        }
        _asyncOperation.allowSceneActivation = true;
    }
}

public enum GameOverState
{
    Win = 0,
    Loss = 1
}