using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toots : SimpleStateMachine
{
    protected override void Start()
    {
        base.Start();
        EnterState("IdleState");
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
            EnterState("BathroomState");
        }
    }
    protected void BathroomState()
    {
        if (enteringState)
        {
            //runs once when state is entered
            Debug.Log("Start Walk to the bathroom.");
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
}
