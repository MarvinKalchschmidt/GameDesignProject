using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TutorialManager : Singleton<TutorialManager>
{
    [SerializeField] private CameraMovement _cameraMovement;
    [SerializeField] private GameObject _tutorialHolder;
    [SerializeField] private TMP_Text _tutorialText;
    [SerializeField] private GameObject _arrowHolder;
    [SerializeField] private bool _trackingShot = true;
    [SerializeField] private string[] _instructions;
    [SerializeField] private TutorialStep[] _tutorialSteps = new TutorialStep[]
        {
            new TutorialStep("Help! The humans are trying to destroy the Tree of Life! Protect the Tree of Life and survive all waves!",null, null, 5f),
            new TutorialStep("Quickly, place a tower near the enemy to defend the Tree of Life! It can only be placed in the green areas!", null, () => towerPlaced, 0f),
            new TutorialStep("Great! You defeated the enemy and got more Bananas!", () => enemyDefeated, null , 0f),
            new TutorialStep("You can move left and right using A and D or your arrow keys!", null, () => Input.GetAxis("Horizontal") > 0, 0f),
            new TutorialStep("You can upgrade your towers to make them stronger and faster buy clicking on them!", () => towerPlaced, () => towerClicked, 0f),
            new TutorialStep("Keep in mind that you can only buy each upgrade once!",() => towerClicked, null, 5f),
            new TutorialStep("If the Tree of Life takes damage, you can heal it by collecting Heal Fruit from the right side of the map!",() => treeTakenDamage, null, 5f),           
        };


    public static event Action OnTutorialStart;
    public static event Action OnTutorialCompleted;

    private int _currentStepIndex;
    private float _stepTimer = 0f;
    private bool _stepCompleted = false;

    private bool tutorialCompleted = false;
    private static bool towerPlaced = false;
    private static bool enemyDefeated = false;
    private static bool towerClicked = false;
    private static bool treeTakenDamage = false;


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
        if(_cameraMovement == null)
        {
            _cameraMovement = GameObject.FindGameObjectWithTag("CameraMovement").GetComponent<CameraMovement>();
        }
        OnTutorialStart?.Invoke();
        DisplayTutorialStep();
        StartCoroutine(StartTrackingShotWithDelay(1));
    }             

    private void Update()
    {
        Debug.Log($"CurrentIndex: {_currentStepIndex}");
        if(tutorialCompleted)
        {
            return;
        }        
       
        if (!_stepCompleted && !_tutorialSteps[_currentStepIndex].HasTimer)
        {
            if (_tutorialSteps[_currentStepIndex].SuccessCondition != null && _tutorialSteps[_currentStepIndex].SuccessCondition())
            {
                _stepCompleted = true;
            }
        }

        if (!_stepCompleted && _tutorialSteps[_currentStepIndex].HasTimer)
        {
            _stepTimer -= Time.deltaTime;
            if (_stepTimer <= 0f)
            {
                _stepCompleted = true;
            }

        }
        if (_stepCompleted)
        {
            _currentStepIndex++;
            if (_currentStepIndex < _tutorialSteps.Length)
            {
                if (_tutorialSteps[_currentStepIndex].OccurCondition != null)
                {
                    if (_tutorialSteps[_currentStepIndex].OccurCondition()){
                        DisplayTutorialStep();
                    }                    
                } 
                else
                {
                    DisplayTutorialStep();
                }
            }
            else
            {
                Debug.Log("Tutorial completed.");
                tutorialCompleted = true;
                OnTutorialCompleted?.Invoke();
            }
        }
    
           
        
            // Update the timer for steps with timers.
            
        
        /*if (tutorialCompleted)
        {
            return;
        }
        if(_instructionIndex == 0)
        {
            ShowTutorialTip();
            StartCoroutine(ShowNextTutorialTipAfterSeconds(5));
        }
        if (_instructionIndex == 1)
        {
            ShowTutorialTip();
            StartCoroutine(ShowNextTutorialTipAfterSeconds(5));
        }
        if (_instructionIndex == 2)
        {
            if (!towerPlaced)
            {
                //StartCoroutine(ShowNextTutorialTipForSeconds(5));
                ShowTutorialTip();
            }
            else
            {
                NextInstruction();
            }
        }
        if(_instructionIndex == 3 && enemyDefeated)
        {
            ShowTutorialTip();
        }*/
    }

    private void DisplayTutorialStep()
    {
        // Display the current tutorial step's text.
        _tutorialText.text = _tutorialSteps[_currentStepIndex].Instructions;
        _tutorialHolder.gameObject.SetActive(true);
        Debug.Log("Instructions:" + _tutorialSteps[_currentStepIndex].Instructions);
        _stepCompleted = false;

        // Start the timer for steps with timers.
        if (_tutorialSteps[_currentStepIndex].HasTimer)
        {
            _stepTimer = _tutorialSteps[_currentStepIndex].TimerDuration;
        }
    }

    private void HideTutotialStep()
    {
        _tutorialHolder.gameObject.SetActive(false);
        _arrowHolder.SetActive(false);
    }

    /*
    private void ShowTutorialTip()
    {
        _tutorialText.text = _instructions[_instructionIndex];
        _tutorialHolder.gameObject.SetActive(true);
    }

    private void ShowTutorialTip(Vector3 arrowLocation, Quaternion rotation)
    {
        _tutorialText.text = _instructions[_instructionIndex];
        _tutorialHolder.gameObject.SetActive(true);
        _arrowHolder.transform.position = arrowLocation;
        _arrowHolder.transform.rotation = rotation;
        _arrowHolder.SetActive(true);
    }

    private void NextInstruction()
    {
        _instructionIndex++;
    }

    private void HideTutotialStep()
    {
        _tutorialHolder.gameObject.SetActive(false);
        _arrowHolder.SetActive(false);
    }
    */
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

    private void TreeTakenDamage()
    {
        treeTakenDamage = true;
    }       

    private IEnumerator StartTrackingShotWithDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _cameraMovement.GetComponent<Animator>().SetTrigger("trackingShot");
    }   
    private class TutorialStep
    {
        private string _instructions;
        private Func<bool> _occurCondition;
        private Func<bool> _successCondition;
        private float _timerDuration;
        private bool _hasTimer;

        public string Instructions { get => _instructions; }
        public System.Func<bool> OccurCondition { get => _occurCondition; }
        public System.Func<bool> SuccessCondition { get => _successCondition; }
        public float TimerDuration { get => _timerDuration; }
        public bool HasTimer { get { return TimerDuration > 0f; } }

        public TutorialStep(string instructions, Func<bool> occurCondition, Func<bool> successCondition, float timerDuration)
        {
            _instructions = instructions;
            _occurCondition = occurCondition;
            _successCondition = successCondition;
            _timerDuration = timerDuration;
        }
        /*
        public TutorialStep(string instructions, Func<bool> occurCondition, float timerDuration)
        {
            _instructions = instructions;
            _occurCondition = occurCondition;
            _successCondition = null;
            _timerDuration = timerDuration;
        }

        public TutorialStep(string instructions, Func<bool> successCondition, float timerDuration)
        {
            _instructions = instructions;
            _occurCondition = null;
            _successCondition = successCondition;
            _timerDuration = timerDuration;
        }*/
    }
}


