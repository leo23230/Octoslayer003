using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwave : MonoBehaviour
{
    public AnimationCurve expansionCurve;

    public float maxScale = 8f;

    public float lerpTime = 3f;

    private float timer = 0f;

    private void Start()
    {

    }

    private void Update()
    {
        timer += Time.deltaTime;

        if(timer > lerpTime)
        {
            timer = lerpTime;
        }
        float lerpRatio = timer / lerpTime;

        float scale = expansionCurve.Evaluate(lerpRatio);

        Vector3 newScale = new Vector3 (scale,scale,scale);
        newScale *= maxScale;

        transform.localScale = newScale;


        if (transform.localScale.x >= maxScale - 0.05f)
        {
            Destroy(gameObject);
        }

    }
}
