using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distraction : MonoBehaviour
{
    public string distractionName;
    public string hint;
    [HideInInspector] public bool distractionTriggered;
    private void Start()
    {
        distractionTriggered = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !distractionTriggered)
        {
            InteractionHint.instance.DisplayHint(hint);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

            if (Input.GetButtonDown("Interact"))
            {
                //Dialogue Trigger ("AWWW HECK")
                distractionTriggered = true;

                //IF THE PLAYER IS VISIBLE
                PlayerStealth.instance.SubtractStealth(20);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InteractionHint.instance.DisableHint();
        }
    }
}
