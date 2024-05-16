using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class Distraction : MonoBehaviour
{
    public string distractionName;
    public string hint;
    [HideInInspector] public bool distractionTriggered;

    [Header("References")]
    public GameObject distractionTarget;
    public GameObject player;
    public GameObject effectPrefab;

    //if the distraction has an animation, set it up with a trigger 
    public Animator anim;
    public string animatorTrigger;

    private bool inTrigger;

    private void Start()
    {
        distractionTriggered = false;
    }
    private void Update()
    {
        if (inTrigger)
        {
            if (Input.GetButtonDown("Interact") && !distractionTriggered)
            {
                //Dialogue Trigger ("AWWW HECK")

                DialogueLua.SetVariable("TootsDrinkSpilled", true);
                DialogueManager.StopConversation();
                DialogueManager.StartConversation("LVL 1", null, null, 31);

                //Trigger Animation
                if (anim != null) anim.SetTrigger(animatorTrigger);

                GetComponent<Effect>().DoEffect(effectPrefab, transform.position, 0.1f);

                distractionTriggered = true;

                InteractionHint.instance.DisableHint();

                if (PlayerStealth.instance.playerIsVisible)
                {
                    PlayerStealth.instance.SubtractStealth(20);
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !distractionTriggered)
        {
            InteractionHint.instance.DisplayHint(hint);
            inTrigger = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InteractionHint.instance.DisableHint();
            inTrigger = false;
        }
    }
}
