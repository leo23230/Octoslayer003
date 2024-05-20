using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class Safe : MonoBehaviour
{
    public GameObject openSafe;
    public GameObject closedSafe;
    public ItemSO password;
    public ItemSO poison;
    public QuestTracker questTracker;

    public void OpenSafeGivePoison()
    {
        closedSafe.SetActive(false);
        openSafe.SetActive(true);
        Inventory.instance.Add(poison);

        QuestLog.SetQuestState("FindTheSafe", QuestState.Success);
        QuestLog.SetQuestState("BackToDel", QuestState.Active);
        if (questTracker != null) questTracker.UpdateTracker();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Inventory.instance.Contains(password))
            {
                InteractionHint.instance.DisplayHint("to unlock the safe");

            }
            else
            {
                InteractionHint.instance.DisplayHint("to unlock the safe once you find the password");
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Inventory.instance.Contains(password))
            {
                if (Input.GetButtonDown("Interact"))
                {
                    OpenSafeGivePoison();
                    InteractionHint.instance.DisableHint();
                }
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
