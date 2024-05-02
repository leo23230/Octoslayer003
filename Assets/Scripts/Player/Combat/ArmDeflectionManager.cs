using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FIMSpace.FTail;

public class ArmDeflectionManager : MonoBehaviour
{
    private TailAnimator2 tailAnimator;
    private void Awake()
    {
        tailAnimator = GetComponent<TailAnimator2>();
        GameObject[] foundShields = GameObject.FindGameObjectsWithTag("Shield");
        Debug.Log("Shields: " + foundShields.Length);
        List<Collider> shieldColliders = new List<Collider>();
        foreach (GameObject gameObject in foundShields)
        {
            Debug.Log("shield found:" + gameObject.name);
            shieldColliders.Add(gameObject.GetComponent<Collider>());
        }
        tailAnimator.IncludedColliders = shieldColliders;

    }
    private void Start()
    {

    }
}
