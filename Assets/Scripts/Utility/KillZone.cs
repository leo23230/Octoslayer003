using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) PlayerStealth.instance.inKillZone = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) PlayerStealth.instance.inKillZone = false;
    }
}
