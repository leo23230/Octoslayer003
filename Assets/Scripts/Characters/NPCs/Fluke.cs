using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using PixelCrushers.DialogueSystem;

public class Fluke : SimpleStateMachine
{
    public GameObject mesh;
    public QuestTracker questTracker;

    protected override void Start()
    {
        base.Start();
        EnterState("IdleState");
        questTracker = GameObject.Find("Dialogue Manager").GetComponent<QuestTracker>();
    }
    protected override void Update()
    {
        base.Update();
    }
    protected override void InitializeStateTable()
    {
        base.InitializeStateTable();
        stateTable.Add("IdleState", IdleState);
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
    
    protected void DeathState()
    {
        if (enteringState)
        {
            //runs once when state is entered
            mesh.transform.DORotate(new Vector3(0, 0, -90), 0.5f);
            mesh.GetComponent<MeshRenderer>().material.DOColor(Color.black, 0.5f);
            enteringState = false;

            //PlayerStealth.instance.SubtractStealth(40);

            //Handle Dialogue
            if (DialogueManager.instance.isConversationActive)
            {
                DialogueManager.StopConversation();
            }
            DialogueLua.SetVariable("FlukeAlive", false);
            DialogueLua.SetVariable("FlukeEscaped", true);
            QuestLog.SetQuestState("FindFluke", QuestState.Success);
            if (questTracker != null) questTracker.UpdateTracker();
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
}
