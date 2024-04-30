using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTarget : MonoBehaviour
{
    public bool stealthOption;

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

    }
}
