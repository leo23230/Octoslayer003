using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FIMSpace.FTail;

public class ArmDeflectionManager : MonoBehaviour
{
    private TailAnimator2 tailAnimator;

    private void Start()
    {
        tailAnimator = GetComponent<TailAnimator2>();
        GameObject[] foundShields = GameObject.FindGameObjectsWithTag("Shield");
        List<Collider> shieldColliders = new List<Collider>();
        foreach (GameObject gameObject in foundShields)
        {
            Debug.Log("shield found:" + gameObject.name);
            shieldColliders.Add(gameObject.GetComponent<Collider>());
        }       
        tailAnimator.IncludedColliders = shieldColliders;
    }
}
