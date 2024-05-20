using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using PixelCrushers.DialogueSystem;

public class Toots : SimpleStateMachine
{
    private DefinedPath definedPath;
    public GameObject mesh;
    public ItemSO itemOfInterest;
    public QuestTracker questTracker;

    protected override void Start()
    {
        base.Start();
        EnterState("IdleState");
        questTracker = GameObject.Find("Dialogue Manager").GetComponent<QuestTracker>();

        definedPath = GetComponent<DefinedPath>();
    }
    protected override void Update()
    {
        base.Update();
    }
    protected override void InitializeStateTable()
    {
        base.InitializeStateTable();
        stateTable.Add("IdleState", IdleState);
        stateTable.Add("BathroomState", BathroomState);
        stateTable.Add("DeathState", DeathState);
    }
    protected void IdleState()
    {
        if (enteringState)
        {
            //runs once when state is entered
            Debug.Log("Entering Idle");
            enteringState = false;
        }
        else if (exitingState)
        {
            //runs once when state is being switched
            Debug.Log("Exiting Idle");
            exitingState = false;
        }
        else
        {
            Debug.Log("Doing Idle");
            //EnterState("BathroomState");
        }
    }
    protected void BathroomState()
    {
        if (enteringState)
        {
            //runs once when state is entered
            Debug.Log("Start Walk to the bathroom.");
            definedPath.FollowPath(0);
            enteringState = false;
        }
        else if (exitingState)
        {
            //runs once when state is being switched
            Debug.Log("I must be dead.");
            exitingState = false;
        }
        else
        {
            Debug.Log("On my way to the bathroom.");
        }
    }
    protected void DeathState()
    {
        if (enteringState)
        {
            //runs once when state is entered
            mesh.transform.DORotate(new Vector3(0, 0, -90), 0.5f);
            mesh.GetComponent<MeshRenderer>().material.DOColor(Color.black, 0.5f);
            enteringState = false;

            PlayerStealth.instance.SubtractStealth(40);

            definedPath.StopFollow();

            //Handle Dialogue
            if (DialogueManager.instance.isConversationActive)
            {
                DialogueManager.StopConversation();
            }
            DialogueLua.SetVariable("TootsAlive", false);

            if (!Inventory.instance.Contains(itemOfInterest)) Inventory.instance.Add(itemOfInterest);
        }
        else if (exitingState)
        {
            //runs once when state is being switched
            exitingState = false;
        }
        else
        {

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(itemOfInterest != null && m_currentState == BathroomState)
        {
            if (other.tag == "Player")
            {
                InteractionHint.instance.DisplayHint("yoink Toot's ID card.");
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (itemOfInterest != null && m_currentState == BathroomState)
        {
            if (other.tag == "Player")
            {
                if (Input.GetButtonDown("Interact"))
                {
                    Inventory.instance.Add(itemOfInterest);
                    InteractionHint.instance.DisableHint();
                    itemOfInterest = null;

                    QuestLog.SetQuestState("TootsIdCard", QuestState.Success);
                    if(questTracker != null) questTracker.UpdateTracker();
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (itemOfInterest != null && m_currentState == BathroomState)
        {
            if (other.tag == "Player")
            {
                InteractionHint.instance.DisableHint();
            }
        }
    }
}
