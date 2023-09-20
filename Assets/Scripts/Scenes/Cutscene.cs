using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;


public class Cutscene : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private VideoPlayer _videoPlayer;

    private AsyncOperation _asyncOperation;

    private void Start()
    {
        StartCoroutine(FadeInButton());
        PreloadGame();
    }

    private void OnEnable()
    {
        _videoPlayer.loopPointReached += VideoFinished;
    }

    private void OnDisable()
    {
        _videoPlayer.loopPointReached -= VideoFinished;
    }

    private IEnumerator FadeInButton()
    {
        yield return new WaitForSeconds(5);
        _button.GetComponent<FadeComponent>().FadeIn();
    }

    private void VideoFinished(VideoPlayer vp)
    {
        Skip();
    }

    public void Skip()
    {
        _asyncOperation.allowSceneActivation = true;
    }

    private void PreloadGame()
    {
        _asyncOperation = SceneManager.LoadSceneAsync("GameScene");
        _asyncOperation.allowSceneActivation = false;
    }
}
