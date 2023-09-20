using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeComponent : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _duration;
    [SerializeField] private float _fadePause = 2;

    void Start()
    {
        if(_canvasGroup == null)
        {
        _canvasGroup = GetComponent<CanvasGroup>();
        }
    }

    public void Fade()
    {   
        StartCoroutine(FadeInOut());
    }

    public void FadeIn()
    {
        StartCoroutine(FadeInStay());
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutStay());
    }

    private IEnumerator FadeInOut()
    {
        yield return Fade(_canvasGroup, 0, 1);

        yield return new WaitForSeconds(_fadePause);

        yield return Fade(_canvasGroup, 1, 0);

    }

    private IEnumerator FadeInStay()
    {
        yield return Fade(_canvasGroup, 0, 1);
    }

    private IEnumerator FadeOutStay()
    {
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