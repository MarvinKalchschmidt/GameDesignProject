using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndSceneManager : MonoBehaviour
{
    [SerializeField] private Image _imageWin;
    [SerializeField] private Image _imageLose;
    [SerializeField] private Button _newGameButton;
    [SerializeField] private Button _quitGameButton;
    [SerializeField] private TMP_Text _loadingText;
    [SerializeField] private TMP_Text _contributorsText;
    [SerializeField] private string[] _contributors;
    [SerializeField] private string _sceneName = "GameScene";


    private AsyncOperation _asyncOperation;

    private void Awake()
    {
        _newGameButton.onClick.AddListener(RestartGame);
        _quitGameButton.onClick.AddListener(QuitGame);
        _contributorsText.text = GenerateContributorsText();
        DisplayWinLoseImage(true);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += PreloadScene;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= PreloadScene;
    }

    private void PreloadScene(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "EndScene")
        {
            StartCoroutine(LoadAsyncScene());
        }
    }

    private void RestartGame()
    {
        Debug.Log("New Game.");
        _asyncOperation.allowSceneActivation = true;
    }

    private void QuitGame()
    {
        Debug.Log("Quit Game.");
        Application.Quit();
    }

    private IEnumerator LoadAsyncScene()
    {
        yield return null;

        _asyncOperation = SceneManager.LoadSceneAsync(_sceneName);
        _asyncOperation.allowSceneActivation = false;

        while (!_asyncOperation.isDone)
        {
            _loadingText.text = "Loading progress: " + (_asyncOperation.progress * 100) + "%";

            if (_asyncOperation.progress >= 0.9f)
            {
                _loadingText.text = "Loading of Tower Defense completed.";
            }

            yield return null;
        }
    }

    private void DisplayWinLoseImage(bool win)
    {
        if (win)
        {
            _imageWin.gameObject.SetActive(true);
        }
        else
        {
        _imageLose.gameObject.SetActive(true);
        }
    }
    
    private string GenerateContributorsText()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append("by:").AppendLine();
        foreach (string contributor in _contributors)
        {
            builder.Append(contributor).Append(",").AppendLine();
        }      

        return builder.ToString();
    }
}