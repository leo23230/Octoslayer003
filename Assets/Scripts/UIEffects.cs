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
    //speedLines
    public GameObject speedLines;
    private Image speedLinesImage;
    private Animator speedLinesAnimator;

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
        speedLines.SetActive(true);
        speedLinesImage = speedLines.GetComponent<Image>();
        speedLinesAnimator = speedLines.GetComponent<Animator>();

        blackScreen.CrossFadeAlpha(0, 0.1f, false);
        redScreen.CrossFadeAlpha(0, 0.01f, false);
    }
    private void Update()
    {
        speedLinesImage.color = new Color(speedLinesImage.color.r, speedLinesImage.color.g, speedLinesImage.color.b, 0);
    }

    public void FadeScreen(float _duration)
    {
        StartCoroutine(FadeInOut(_duration));
    }
    public void RedFlash(float _duration)
    {
        StartCoroutine(RedScreenFlash(_duration));
    }
    public void DashLines()
    {
        speedLinesAnimator.SetTrigger("dash");
    }
    public void LeftWallLines()
    {
        //set bool
    }
    public void RightWallLines()
    {
        //set bool
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
