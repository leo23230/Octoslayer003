using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : MonoBehaviour
{
    public ItemSO requiredItem;
    public GameObject door;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Inventory.instance.Contains(requiredItem))
            {
                InteractionHint.instance.DisplayHint("open door");
            }
            else
            {
                InteractionHint.instance.DisplayHint("open door after you aquire the keycard.");
            }
                
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetButtonDown("Interact") && Inventory.instance.Contains(requiredItem))
            {
                InteractionHint.instance.DisableHint();
                door.SetActive(false);
                gameObject.SetActive(false);
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
