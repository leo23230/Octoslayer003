using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    //References
    [Header("References")]
    GameObject enemyModel;
    Animator anim;
    public GameObject player;
    private Rigidbody rb;

    //state machine
    public string enemyState;
    [HideInInspector] public delegate void EnemyState(); // This defines what type of method you're going to call.
    [HideInInspector] public EnemyState m_currentState; // This is the variable holding the method you're going to call.

    bool enteringState = true;
    bool vulnerable = false;
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
        SwitchState("Combat");
    }

    private void OnEnable()
    {
        StaticEventHandler.OnPlayerAttack += PlayerAttackReaction;
    }
    private void OnDisable()
    {
        StaticEventHandler.OnPlayerAttack -= PlayerAttackReaction;
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
        //waiting until persue conditions are met
    }
    private void Persue()
    {
        if (enteringState)
        {
            //runs once when state is entered

            anim.SetBool("combat", false);
            anim.SetBool("run", true);
            StopCoroutine(kickCoroutine);

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

            enteringState = false;
        }
        
    }
    private void Block()
    {
        if (enteringState)
        {
            //runs once when state is entered

            //anim.SetBool("block", true);

            StopCoroutine(kickCoroutine);

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

    public void PlayerAttackReaction(PlayerAttackEventArgs eventArgs)
    {
        if (!eventArgs.attack.isSpecial)
        {
            if(enemyState != "Persue")
            {
                if (vulnerable)
                {
                    //TakeDamage();
                    anim.SetTrigger("hurt");
                }
                else
                {
                    anim.SetBool("block", true);
                    SwitchState("Block");
                }
            }
        }
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
}
