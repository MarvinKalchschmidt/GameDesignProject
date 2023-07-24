using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeComponent : MonoBehaviour
{    
    private CanvasGroup _canvasGroup;
    [SerializeField] private float _duration = 0.4f;
    [SerializeField] private float _fadePause = 2;

    void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Fade()
    {   
        StartCoroutine(FadeInOut());
        //_canvasGroup, _canvasGroup.alpha, _isFadedOut ? 1 : 0)        
    }

    private IEnumerator FadeInOut()
    {
        yield return Fade(_canvasGroup, 0, 1);

        yield return new WaitForSeconds(_fadePause);

        yield return Fade(_canvasGroup, 1, 0);

    }

    private IEnumerator Fade(CanvasGroup canvasGroup, float start, float end)
    {
        float counter = 0f;
        while (counter < _duration)
        {
            counter += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, counter / _duration);
            yield return null;
        }
    }
}