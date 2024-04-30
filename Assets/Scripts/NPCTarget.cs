using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTarget : MonoBehaviour
{
    public bool stealthOptionUsed;
    public Distraction distraction;
    public GameObject bodyDouble;

    private GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
        bodyDouble.SetActive(false);
    }

    private void Update()
    {
        //this is crap code
        if (distraction.distractionTriggered)
        {
            if (!stealthOptionUsed)
            {
                bodyDouble.SetActive(true);
                gameObject.SetActive(false);
                stealthOptionUsed = true;
            }
                
        }
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
        if(Vector3.Distance(transform.position, player.transform.position) <= 5)
        {
            StaticEventHandler.CallPlayerSpottedEvent(player.transform.position);
        }
    }
}
