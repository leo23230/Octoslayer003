using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempWinConditionTwo : MonoBehaviour
{
    public GameObject winScreen;
    private bool inBounds;

    private void Start()
    {
        inBounds = false;
    }
    private void OnEnable()
    {
        StaticEventHandler.OnPlayerAttack += ReactToPlayerAttack;
    }
    private void OnDisable()
    {
        StaticEventHandler.OnPlayerAttack -= ReactToPlayerAttack;
    }

    public void ReactToPlayerAttack(PlayerAttackEventArgs eventArgs)
    {
        if (inBounds)
        {
            winScreen.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inBounds = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inBounds = false;
        }
    }
}
