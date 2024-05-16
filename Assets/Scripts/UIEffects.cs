using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEffects : MonoBehaviour
{
    public static UIEffects instance;

    [Header("References")]

    public Image blackScreen;
    public Image redScreen;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        //setting to active since we have them innactive during development//
        blackScreen.gameObject.SetActive(true);
        redScreen.gameObject.SetActive(true);

        blackScreen.CrossFadeAlpha(0, 0.1f, false);
        redScreen.CrossFadeAlpha(0, 0.01f, false);
    }

    public void FadeScreen(float _duration)
    {
        StartCoroutine(FadeInOut(_duration));
    }
    public void RedFlash(float _duration)
    {
        StartCoroutine(RedScreenFlash(_duration));
    }

    private IEnumerator FadeInOut(float _duration)
    {
        blackScreen.CrossFadeAlpha(1, _duration, false);
        yield return new WaitForSeconds(_duration);
        blackScreen.CrossFadeAlpha(0, _duration, false);
    }

    private IEnumerator RedScreenFlash(float _duration)
    {
        redScreen.CrossFadeAlpha(1, _duration/2, false);
        yield return new WaitForSeconds(_duration);
        redScreen.CrossFadeAlpha(0, _duration/2, false);
    }
}
