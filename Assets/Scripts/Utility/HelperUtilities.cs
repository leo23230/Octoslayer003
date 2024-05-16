using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperUtilities : MonoBehaviour
{
    public static HelperUtilities instance;

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

    public delegate void Method(); // This defines what type of method you're going to call.
    public void InvokeAfterSeconds(Method _method, float _duration)
    {
        StartCoroutine(InvokeAfterSecondsCoroutine(_method, _duration));
    }
    IEnumerator InvokeAfterSecondsCoroutine(Method _method, float _duration)
    {
        yield return new WaitForSeconds(_duration);
        _method();
        yield return null;
    }
    public void InvokeAfterTrue(Method _method, System.Func<bool> _condition)
    {
        StartCoroutine(InvokeAfterTrueCoroutine(_method, _condition));
    }
    IEnumerator InvokeAfterTrueCoroutine(Method _method, System.Func<bool> _condition)
    {
        yield return new WaitUntil(_condition);

        yield return null;
    }
}
