using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [Header("References")]
    public TravelNetwork network;

    [Header("General Details")]
    public string entryPointName;
    [Tooltip("Amount to subtract from player's base stealth score upon entry out of 100.")]
    public int stealthScore;
    [Tooltip("Reason for stealth score, mostly for developers.")]
    public string explanation;
    public string hint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player Entered" + gameObject.name + "Zone");
            InteractionHint.instance.DisplayHint(hint);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player In " + gameObject.name + "Zone");
            if (Input.GetButtonDown("Interact"))
            {
                network.MoveToNextPoint(transform);
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
