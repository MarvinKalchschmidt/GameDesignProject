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

    private void Start()
    {
        _newGameButton.onClick.AddListener(RestartGame);
        _quitGameButton.onClick.AddListener(QuitGame);
        _contributorsText.text = GenerateContributorsText();
        DisplayWinLoseImage(GameManager.GameOverState);
        StartCoroutine(FadeInComponents());
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

    private IEnumerator FadeInComponents()
    {
        yield return new WaitForSeconds(1f);
        if (_imageWin.IsActive()) _imageWin.GetComponent<FadeComponent>().FadeIn();
        if (_imageLose.IsActive()) _imageLose.GetComponent<FadeComponent>().FadeIn();
        _newGameButton.GetComponent<FadeComponent>().FadeIn();
        _quitGameButton.GetComponent<FadeComponent>().FadeIn();
        _loadingText.GetComponent<FadeComponent>().FadeIn();
        _contributorsText.GetComponent<FadeComponent>().FadeIn();
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
                _loadingText.text = "Loading of Djungle Defenders completed.";
            }

            yield return null;
        }
    }

    private void DisplayWinLoseImage(GameOverState gameOverState)
    {
        if (gameOverState == GameOverState.Win)
        {
            _imageWin.gameObject.SetActive(true);
        }
        else if (gameOverState == GameOverState.Loss)
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