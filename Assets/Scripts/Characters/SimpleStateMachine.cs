using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleStateMachine : MonoBehaviour
{
    //state machine
    [HideInInspector] public delegate void State(); // This defines what type of method you're going to call.
    [HideInInspector] public State m_currentState; // This is the variable holding the method you're going to call.
    protected bool enteringState;
    protected bool exitingState;
    protected Dictionary<string, State> stateTable = new Dictionary<string, State>();

    protected virtual void Start()
    {
        InitializeStateTable();
        m_currentState = BaseState; //Starts with base state to avoid unecessary conditionals
    }
    protected virtual void Update()
    {
        RunState(m_currentState);
    }

    //public facing function to switch states
    public void EnterState(string _stateString)
    {
        State _state = stateTable[_stateString];
        if(_state != null)
        {
            if (m_currentState != _state)
            {
                //begin exit state this frame
                exitingState = true;
                StartCoroutine(SwitchState(_state));
            }
        }
        else
        {
            throw new System.Exception("The state " + _stateString + "does not exist.");
        }
    }
    public bool IsCurrentState(string _state)
    {
        return m_currentState == stateTable[_state];
    }
    //each state has to override this function
    protected virtual void InitializeStateTable()
    {
        stateTable.Add("BaseState", BaseState);
    }
    private IEnumerator SwitchState(State _state)
    {
        //wait for current state to exit
        while(exitingState)
        {
            yield return null;
        }

        //reset this bool so enter state code can run in the method
        enteringState = true;
        m_currentState = _state;

        yield return null;
    }
    private void RunState(State _state)
    {
        _state();
    }
    private void BaseState()
    {
        //Must be this way to ensure enter and exit only happen once in the event of an EnterState Call inside a state
        if (enteringState)
        {
            //runs once when state is entered
            enteringState = false;
        }
        else if (exitingState)
        {
            //runs once when state is being switched
            exitingState = false;
        }
        else
        {
            //runs every frame while in this state
        }
    }
}