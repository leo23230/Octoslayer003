using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class EnemyBehaviour : MonoBehaviour
{
    //References
    [Header("References")]
    private GameObject enemyModel;
    private Animator anim;
    public GameObject player;
    public SkinnedMeshRenderer meshRenderer;
    private Material[] mats;
    private Rigidbody rb;
    private NavMeshAgent navAgent;
    private Health healthComponent;

    //state machine
    public string enemyState;
    [HideInInspector] public delegate void EnemyState(); // This defines what type of method you're going to call.
    [HideInInspector] public EnemyState m_currentState; // This is the variable holding the method you're going to call.

    bool enteringState = true;
    bool vulnerable = false;
    bool playerSpotted = false;
    float attackFrequency = 4f;
    Coroutine kickCoroutine;

    [Header("EnemyParameters")]
    public float runSpeed = 4f;
    public float combatSpeed = 2f;

    //acceptable combat range
    public float minRange = 1f;
    public float maxRange = 4f;

    private void Start()
    {
        enemyModel = transform.Find("EnemyModel").gameObject;
        anim = enemyModel.GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        navAgent = GetComponent<NavMeshAgent>();
        mats = meshRenderer.materials;

        //Health
        healthComponent = GetComponent<Health>();
        healthComponent.SetStartingHealth(100);

        //Start in Idle
        SwitchState("Idle");
    }

    private void OnEnable()
    {
        StaticEventHandler.OnPlayerAttack += PlayerAttackReaction;
        StaticEventHandler.OnPlayerSpotted += PlayerSpottedReaction;
    }
    private void OnDisable()
    {
        StaticEventHandler.OnPlayerAttack -= PlayerAttackReaction;
        StaticEventHandler.OnPlayerSpotted -= PlayerSpottedReaction;
    }

    private void Update()
    {
        RunState(m_currentState);
    }

    private void EnterState(EnemyState _state)
    {
        //reset this bool so enter state code can run in the method
        Debug.Log("Entering " + enemyState + " State");
        enteringState = true;
        m_currentState = _state;
    }

    private void RunState(EnemyState _state)
    {
        _state();
    }

    public void SwitchState(string _enemyState)
    {
        //if not already in the state, switch to that state
        if(_enemyState != enemyState)
        {
            enemyState = _enemyState;
            StateHandler();
        }
    }

    //template for states
    private void BaseState()
    {
        if (enteringState)
        {
            //runs once when state is entered
            enteringState = false;
        }
        //rest of state//
    }

    //states//
    private void Idle()
    {
        if (enteringState)
        {
            //runs once when state is entered
            enteringState = false;
        }
        //if player is spotted, call player spotted event to alert guards within a radius of the player

    }
    private void Persue()
    {
        if (enteringState)
        {
            //runs once when state is entered

            anim.SetBool("combat", false);
            anim.SetBool("run", true);
            if(kickCoroutine != null) StopCoroutine(kickCoroutine);
            if(navAgent.isStopped) navAgent.isStopped = false;

            //if player has not been spotted yet, alert other gaurds
            if (!playerSpotted)
            {
                StaticEventHandler.CallPlayerSpottedEvent(player.transform.position);
                playerSpotted = true;
            }

            enteringState = false;
        }
        //persue player until within range
        if (IsWithinRange(player, minRange, maxRange))
        {
            SwitchState("Combat");
        }
        else
        {
            float distanceToPlayer= Vector3.Distance(transform.position, player.transform.position);
            float averageOfRange = maxRange + minRange / 2;
            if(!Mathf.Approximately(distanceToPlayer, averageOfRange))
            {
                MoveTowards(player, runSpeed);
            }
            else
            {
                SwitchState("Combat");
            }
        }
    }
    private void Combat()
    {

        if (enteringState)
        {
            //runs once when state is entered
            anim.SetBool("run", false);
            anim.SetBool("combat", true);

            kickCoroutine = StartCoroutine(KickRepeat());

            enteringState = false;
        }
        transform.LookAt(new Vector3 (player.transform.position.x, transform.position.y, player.transform.position.z));
        if (!IsWithinRange(player, minRange, maxRange))
        {
            SwitchState("Persue");
        }
        else
        {
        }

    }
    private void Hurt()
    {
        if (enteringState)
        {
            //runs once when state is entered

            anim.SetTrigger("hurt");

            StartCoroutine(Flash(0.2f));

            enteringState = false;
            //navAgent.isStopped = true;
        }
    }
    public void TakeDamage(float _damage, float _knockback)
    {
        //start damage 
        healthComponent.SubtractHealth(_damage);      

        if(healthComponent.GetHealth() <= 0)
        {
            SwitchState("Dead");
        }
        else
        {
            SwitchState("Hurt");
            navAgent.Warp(transform.position - transform.forward * _knockback);
            transform.DOShakePosition(0.3f, 0.3f, 1);
        }
    }
    private IEnumerator Flash(float _duration)
    {
        foreach (Material mat in mats)
        {
            mat.DOColor(Color.magenta, _duration / 2);   
        }
        yield return new WaitForSeconds(_duration + _duration/2);
        foreach (Material mat in mats)
        {
            mat.DOColor(Color.white, _duration / 2);
        }

        yield return null;
    }
    private void Block()
    {
        if (enteringState)
        {
            //runs once when state is entered

            //anim.SetBool("block", true);

            if(kickCoroutine != null) StopCoroutine(kickCoroutine);

            enteringState = false;
        }
    }

    private void Stunned()
    {
        if (enteringState)
        {
            //runs once when state is entered

            anim.SetTrigger("stunned");

            enteringState = false;
        }
    }

    private IEnumerator deathFlicker()
    {
        foreach (Material mat in mats)
        {
            mat.DOColor(Color.magenta, 0.05f);
        }
        yield return new WaitForSeconds(0.1f);
        foreach (Material mat in mats)
        {
            mat.DOColor(Color.white, 0.05f);
        }
        yield return new WaitForSeconds(0.1f);
        foreach (Material mat in mats)
        {
            mat.DOColor(Color.magenta, 0.05f);
        }
        yield return new WaitForSeconds(0.1f);
        foreach (Material mat in mats)
        {
            mat.DOColor(Color.white, 0.05f);
        }
        yield return new WaitForSeconds(0.1f);
        foreach (Material mat in mats)
        {
            mat.DOColor(Color.black, 2f);
        }

        yield return null;
    }

    private void Dead()
    {
        if (enteringState)
        {
            //runs once when state is entered

            anim.SetTrigger("dead");

            //Disable Collider
            Collider col = GetComponent<Collider>();
            col.enabled = false;

            StartCoroutine(deathFlicker());

            enteringState = false;
        }
    }

    public void PlayerAttackReaction(PlayerAttackEventArgs eventArgs)
    {
        if (!eventArgs.attack.isSpecial)
        {
            if(IsWithinRange(player, 0.2f, 7))
            {
                if (enemyState != "Persue")
                {
                    if (enemyState == "Combat" || enemyState == "Idle")
                    {
                        anim.SetBool("block", true);
                        SwitchState("Block");
                    }
                }
            }
        }
    }
    public void PlayerSpottedReaction(PlayerSpottedEventArgs eventArgs) 
    {
        /*if(IsWithinRange(player, 0.5f, 30f))
        {

        }*/
        SwitchState("Persue");
    }

    private IEnumerator KickRepeat()
    {
        while(enemyState == "Combat")
        {
            var rand = Random.Range(1.5f, attackFrequency);
            yield return new WaitForSeconds(rand);

            anim.SetTrigger("kick");

            yield return null;
        }
        yield return null;
    }
    //relates string to method and calls enter state
    private void StateHandler()
    {
        if(enemyState == "Idle")
        {
            EnterState(Idle);
        }
        else if (enemyState == "Persue")
        {
            EnterState(Persue);
        }
        else if (enemyState == "Combat")
        {
            EnterState(Combat);
        }
        else if (enemyState == "Block")
        {
            EnterState(Block);
        }
        else if (enemyState == "Hurt")
        {
            EnterState(Hurt);
        }
        else if (enemyState == "Stunned")
        {
            EnterState(Stunned);
        }
        else if (enemyState == "Dead")
        {
            EnterState(Dead);
        }
    }

    //Helpers
    private bool IsWithinRange(GameObject _object, float _minRange, float _maxRange)
    {
        Vector3 myPosition = gameObject.transform.position;
        Vector3 objectPosition = _object.transform.position;
        float distanceToObject = Vector3.Distance(myPosition, objectPosition);

        // if 3 > DTO < 5 it is in range

        if(distanceToObject < _maxRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void MoveTowards(GameObject _target, float _speed)
    {
        NavMeshAgent navAgent = GetComponent<NavMeshAgent>();

        //transform.LookAt(_target.transform.position);
        //transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.y, transform.rotation.z));
        Debug.DrawRay(transform.position, _target.transform.position, Color.green);
        Vector3 target = new Vector3(_target.transform.position.x, transform.position.y, _target.transform.position.z);
        navAgent.SetDestination(target);

        /*Vector3 target = new Vector3(_target.transform.position.x, transform.position.y, _target.transform.position.z);
        Vector3 destination = transform.position + target;
        transform.position = Vector3.MoveTowards(transform.position, destination, _speed * Time.deltaTime);*/
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && enemyState == "Combat")
        {
            player.GetComponent<PlayerCombat>().TakeDamage(20);
        }
    }
}
