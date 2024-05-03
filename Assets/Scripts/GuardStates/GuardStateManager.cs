using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardStateManager : MonoBehaviour
{
    [HideInInspector] public GuardBaseState currentState;
    [HideInInspector] public GuardPatrolState patrolState;
    [HideInInspector] public GuardChaseState chaseState;
    [HideInInspector] public GuardDeathState deathState;

    //Components//
    public Animator animator;
    public Collider col;
    public Transform player;


    void Start()
    {
        //reference caching//
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        //state machine stuff//
        patrolState = GetComponent<GuardPatrolState>();
        chaseState = GetComponent<GuardChaseState>();
        deathState = GetComponent<GuardDeathState>();
        currentState = patrolState;

        currentState.EnterState(this);

    }

    void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(GuardBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

   
}
