using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialManager : Singleton<TutorialManager>
{
    [SerializeField] private CameraMovement _cameraMovement;
    [SerializeField] private GameObject _tutorialHolder;
    [SerializeField] private TMP_Text _tutorialText;
    [SerializeField] private GameObject _arrowHolder;
    [SerializeField] private bool _trackingShot = true;
    [SerializeField] private TutorialStep[] _tutorialSteps = new TutorialStep[]
        {
            new TutorialStep("Help! The humans are trying to destroy the Tree of Life! Protect the Tree of Life and survive all waves!", null, 6f),
            new TutorialStep("Quickly, place a tower near the Tree of Life to defend it! It can only be placed in the green areas!", () => towerPlaced, 0f, new Vector3(0, -300), Quaternion.identity),
            new TutorialStep("By defeating enemies and get more Bananas! You can use them to buy and upgrade more towers!", () => enemyDefeated , 0f, new Vector3(-800, 350), Quaternion.Euler(0, 0, 180)),
            new TutorialStep("You can move left and right using A and D or your arrow keys!", () => Input.GetAxis("Horizontal") > 0f, 0f),
            new TutorialStep("You can upgrade your towers to make them stronger and faster buy clicking on them! Click on your tower!", () => towerClicked, 0f),
            new TutorialStep("Keep in mind that you can only buy each upgrade once!", null, 5f),
            new TutorialStep("If the Tree of Life takes damage, you can heal it by collecting Heal Fruit from the right side of the map!", null, 5f, new Vector3(0, 350), Quaternion.Euler(0, 0, 180))          
        };

    
    public static event Action OnTutorialStart;
    public static event Action OnTutorialCompleted;

    private int _currentStepIndex = 0;
    private float _stepTimer = 0f;
    public bool _stepCompleted = false;

    private bool tutorialStarted = false;
    private static bool towerPlaced = false;
    private static bool enemyDefeated = false;
    private static bool towerClicked = false;


    private void OnEnable()
    {
        TowerPlacementManager.OnTowerPlaced += TowerPlaced;
        Tower.OnClickTower += TowerClicked;
        Enemy.OnEnemyDiesFromDamage += EnemyDefeated;
    }

    private void OnDisable()
    {
        TowerPlacementManager.OnTowerPlaced -= TowerPlaced;
        Tower.OnClickTower -= TowerClicked;
        Enemy.OnEnemyDiesFromDamage -= EnemyDefeated;  
    }

    private void Start()
    {
        if (_cameraMovement == null)
        {
            _cameraMovement = GameObject.FindGameObjectWithTag("CameraMovement").GetComponent<CameraMovement>();
        }
        tutorialStarted = true;
        OnTutorialStart?.Invoke();
        DisplayTutorialStep();
        StartCoroutine(StartTrackingShotWithDelay(1));
    }
        

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (!tutorialStarted || GameManager.SkipTutorial)
        {
            return;
        }        
       
        if (!_stepCompleted && !_tutorialSteps[_currentStepIndex].HasTimer)
        {
            if (_tutorialSteps[_currentStepIndex].SuccessCondition != null && _tutorialSteps[_currentStepIndex].SuccessCondition())
            {
                HideTutotialStep();
                _stepCompleted = true;
            }
        }
       

        if (!_stepCompleted && _tutorialSteps[_currentStepIndex].HasTimer)
        {
            _stepTimer -= Time.deltaTime;
            if (_stepTimer <= 0f)
            {
                HideTutotialStep();
                _stepCompleted = true;
            }
        }

        if (_stepCompleted)
        {
            _currentStepIndex++;
            if (_currentStepIndex < _tutorialSteps.Length)
            {
                _stepCompleted = false;
                DisplayTutorialStep(); 
            }
            else
            {
                Debug.Log("Tutorial completed.");
                tutorialStarted = false;
                GameManager.SkipTutorial = true;
                OnTutorialCompleted?.Invoke();
            } 
        }        
    }

    private void DisplayTutorialStep()
    {        
        _tutorialText.text = _tutorialSteps[_currentStepIndex].Instructions;
        _tutorialHolder.gameObject.SetActive(true);
        Debug.Log("Instructions:" + _tutorialSteps[_currentStepIndex].Instructions);

        if (_tutorialSteps[_currentStepIndex].HasTimer)
        {
            _stepTimer = _tutorialSteps[_currentStepIndex].TimerDuration;
        }

        Vector3 arrowLocation = _tutorialSteps[_currentStepIndex].ArrowPosition;
        Quaternion arrowRotation = _tutorialSteps[_currentStepIndex].ArrowRotation;
        
        if (arrowLocation != Vector3.zero)
        {
            _arrowHolder.GetComponent<RectTransform>().localPosition = arrowLocation;
            _arrowHolder.GetComponent<RectTransform>().localRotation = arrowRotation;
            _arrowHolder.SetActive(true);
        } 
        else
        {
            _arrowHolder.SetActive(false);
        }
    }
   

    private void HideTutotialStep()
    {
        _tutorialHolder.gameObject.SetActive(false);
        _arrowHolder.SetActive(false);        
    }
        
    private void TowerPlaced(int _)
    {
        towerPlaced = true;
    }

    private void EnemyDefeated(int _)
    {
        enemyDefeated = true;
    }

    private void TowerClicked()
    {
        towerClicked = true;
    }       

    private IEnumerator StartTrackingShotWithDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _cameraMovement.GetComponent<Animator>().SetTrigger("trackingShot");
    }   

    private class TutorialStep
    {
        private string _instructions;
        private Func<bool> _successCondition;
        private Vector3 _arrowPosition;
        private Quaternion _arrowRotation;
        private float _timerDuration;

        public string Instructions { get => _instructions; }
        public Func<bool> SuccessCondition { get => _successCondition; }
        public float TimerDuration { get => _timerDuration; }
        public Vector3 ArrowPosition { get { return _arrowPosition; } }
        public Quaternion ArrowRotation { get { return _arrowRotation; } }
        public bool HasTimer { get { return TimerDuration > 0f; } }

        public TutorialStep(string instructions, Func<bool> successCondition, float timerDuration)
        {
            _instructions = instructions;
            _successCondition = successCondition;
            _timerDuration = timerDuration;
        }

        public TutorialStep(string instructions, Func<bool> successCondition, float timerDuration, Vector3 arrowPosition, Quaternion arrowRotation)
        {
            _instructions = instructions;
            _successCondition = successCondition;
            _timerDuration = timerDuration;
            _arrowPosition = arrowPosition;
            _arrowRotation = arrowRotation;
        }
    }
}


