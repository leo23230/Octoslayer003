using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEffects : MonoBehaviour
{
    public static UIEffects instance;

    [Header("References")]

    public Image blackScreen;

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
        blackScreen.CrossFadeAlpha(0, 0.1f, false);
    }

    public void FadeScreen(float _duration)
    {
        StartCoroutine(FadeInOut(_duration));
    }

    private IEnumerator FadeInOut(float _duration)
    {
        blackScreen.CrossFadeAlpha(1, _duration, false);
        yield return new WaitForSeconds(_duration);
        blackScreen.CrossFadeAlpha(0, _duration, false);
    }
}
