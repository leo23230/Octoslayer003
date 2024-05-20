using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using PixelCrushers.DialogueSystem;

public class DelBossSM : SimpleStateMachine, IDamageable
{
    [Header("Attacks")]
    public List<EnemyAttackSO> phaseOneAttacks = new List<EnemyAttackSO>();
    public List<EnemyAttackSO> phaseTwoAttacks = new List<EnemyAttackSO>();
    public List<EnemyAttackSO> closeAttacks = new List<EnemyAttackSO>();
    public EnemyAttackSO closeAttack;

    [Header("Stats")]
    public int phaseOneHealth = 200;
    public int phaseOneAttackCooldown = 5;
    public int phaseTwoHealth = 300;
    public int phaseTwoAttackCooldown = 3;
    public float runSpeed;
    private float minRange = 1f;
    private float maxRange = 6f;

    [Header("References")]
    public Animator animator;
    public GameObject player;
    //public SkinnedMeshRenderer meshRenderer;
    public List<SkinnedMeshRenderer> meshes = new List<SkinnedMeshRenderer>();
    public Rigidbody rb;
    private List<Material> mats = new List<Material>();
    private List<Color> colors = new List<Color>();
    private Health healthComponent;
    public NavMeshAgent navAgent;
    public GameObject phaseTwoEffect;
    public GameObject winScreen;
    public GameObject healthBar;

    private bool isResting;
    public EnemyAttackSO currentAttack;
    private List<Vector3> oldPlayerPositions = new List<Vector3>();
    private float smoothSpeed = 0.125f;
    private Vector3 turnVelocity = Vector3.zero;

    private Coroutine damageOverTime;

    protected override void Start()
    {
        base.Start();
        EnterState("IdleState");

        //refs
        healthComponent = GetComponent<Health>();
        healthComponent.SetStartingHealth(phaseOneHealth + phaseTwoHealth);
        navAgent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        foreach(SkinnedMeshRenderer mesh in meshes)
        {
            mats.AddRange(mesh.materials);
        }
        SaveMaterialColors();
       // Debug.Log("Mats " + mats.Count);
        
    }
    protected override void Update()
    {
        base.Update();
    }
    protected override void InitializeStateTable()
    {
        base.InitializeStateTable();
        stateTable.Add("IdleState", IdleState);
        stateTable.Add("CombatPhaseOneState", CombatPhaseOneState);
        stateTable.Add("CombatPhaseTwoState", CombatPhaseTwoState);
        stateTable.Add("DeadState", DeadState);
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
            Debug.Log("Doing DEL Idle");
            EnterState("CombatPhaseOneState");
        }
    }
    protected void HurtState()
    {
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
            
        }
    }
    protected void DeadState()
    {
        if (enteringState)
        {
            //runs once when state is entered
            animator.SetTrigger("death");
            healthBar.SetActive(false);

            StartCoroutine(WinScreen());

            enteringState = false;
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
    private IEnumerator WinScreen()
    {
        yield return new WaitForSeconds(5f);
        winScreen.SetActive(true);
    }
    protected void CombatPhaseOneState()
    {
        if (enteringState)
        {
            Debug.Log("Entering DEL Combat");
            //runs once when state is entered
            StartCoroutine(PhaseSequence(phaseOneAttacks, phaseOneAttackCooldown));
            enteringState = false;
        }
        else if (exitingState)
        {
            //runs once when state is being switched
            exitingState = false;
        }
        else
        {
            oldPlayerPositions.Add(player.transform.position);
            float frameRate = Mathf.Round(1f / Time.deltaTime);
            if (oldPlayerPositions.Count > 30)
            {
                oldPlayerPositions.RemoveAt(0);
            }

            if (isResting || currentAttack.attackName == "Laser")
            {
                Vector3 smoothedPosition = Vector3.SmoothDamp(oldPlayerPositions[0], player.transform.position, ref turnVelocity, smoothSpeed);
                Vector3 lookVector = new Vector3(oldPlayerPositions[0].x, transform.position.y, oldPlayerPositions[0].z);
                transform.LookAt(lookVector);
            }
        }
    }
    protected void CombatPhaseTwoState()
    {
        if (enteringState)
        {
            Debug.Log("Entering PHASE TWO");


            navAgent.speed += 1;
            StartCoroutine(PhaseTwoEffect());
            StartCoroutine(PhaseSequence(phaseTwoAttacks, phaseTwoAttackCooldown));


            enteringState = false;
        }
        else if (exitingState)
        {
            //runs once when state is being switched
            exitingState = false;
        }
        else
        {
            oldPlayerPositions.Add(player.transform.position);
            float frameRate = Mathf.Round(1f / Time.deltaTime);
            if (oldPlayerPositions.Count > 30)
            {
                oldPlayerPositions.RemoveAt(0);
            }

            if (isResting || currentAttack.attackName == "Laser")
            {
                Vector3 smoothedPosition = Vector3.SmoothDamp(oldPlayerPositions[0], player.transform.position, ref turnVelocity, smoothSpeed);
                Vector3 lookVector = new Vector3(oldPlayerPositions[0].x, transform.position.y, oldPlayerPositions[0].z);
                transform.LookAt(lookVector);
            }
        }
    }

    IEnumerator PhaseTwoEffect()
    {
        GameObject effect = Instantiate(phaseTwoEffect);
        effect.transform.position = transform.position;
        yield return new WaitForSeconds(3);
        Destroy(effect);
        yield break;
    }

    IEnumerator Pursue()
    {
        while (isResting)
        {
            if (IsWithinRange(player, minRange, maxRange))
            {
                animator.SetBool("run", false);
                Debug.Log("Resting in Idle");

                //do nothing
            }
            else
            {
                //animation
                animator.SetBool("run", true);

                float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
                float averageOfRange = maxRange + minRange / 2;
                MoveTowards(player, runSpeed);
            }

            yield return null;
        }

        //always make sure it is false before leaving
        animator.SetBool("run", false);

        yield break;
    }
    private IEnumerator PhaseSequence(List<EnemyAttackSO> _attacks, int _timeBetweenAttacks)
    {
        //pick an attack
        while(healthComponent.GetHealth() > healthComponent.GetStartingHealth() - phaseOneHealth)
        {

            EnemyAttackSO chosenAttack;

            if (!isResting)
            {
                Debug.Log("Doing Move");

                //logic for choosing an attack
                if (IsWithinRange(player, minRange, maxRange))
                {
                    int rand = Mathf.RoundToInt(Random.Range(0, closeAttacks.Count));
                    chosenAttack = closeAttacks[rand];
                }
                else
                {
                    int rand = Mathf.RoundToInt(Random.Range(0, phaseOneAttacks.Count));
                    chosenAttack = _attacks[rand];
                }
                currentAttack = chosenAttack;

                animator.SetBool(chosenAttack.animTrigger, true);

                StartCoroutine(PhaseTimer(chosenAttack, _timeBetweenAttacks));
            }

            yield return null;
        }

        EnterState("CombatPhaseTwoState");

        //GARBAGE TEMP CODE WILL BE REPLACED//
        while (healthComponent.GetHealth() > 0 && healthComponent.GetHealth() <= healthComponent.GetStartingHealth() - phaseOneHealth)
        {
            EnemyAttackSO chosenAttack;

            if (!isResting)
            {
                Debug.Log("Doing Move");

                //logic for choosing an attack
                if (IsWithinRange(player, minRange, maxRange))
                {
                    int rand = Mathf.RoundToInt(Random.Range(0, closeAttacks.Count));
                    chosenAttack = closeAttacks[rand];
                }
                else
                {
                    int rand = Mathf.RoundToInt(Random.Range(0, phaseOneAttacks.Count));
                    chosenAttack = _attacks[rand];
                }
                currentAttack = chosenAttack;

                animator.SetBool(chosenAttack.animTrigger, true);

                StartCoroutine(PhaseTimer(chosenAttack, _timeBetweenAttacks));
            }

            yield return null;
        }

        yield break;
    }

    IEnumerator PhaseTimer(EnemyAttackSO chosenAttack, float _timeBetweenAttacks)
    {

        //indicate boss is resting
        isResting = true;

        //Wait until the attack is over
        //this way the time between attacks is more consistant 
        if (chosenAttack.endsWithAnimation)
        {
            yield return new WaitUntil(() => !animator.GetBool(chosenAttack.animTrigger));
            Debug.Log("Attack Over");
        }
        else
        {
            yield return new WaitForSeconds(chosenAttack.duration);
            Debug.Log("Attack Over");
        }

        //start pursue coroutine
        StartCoroutine(Pursue());

        yield return new WaitForSeconds(_timeBetweenAttacks);

        //reset isResting to stop pursuing player
        isResting = false;

        yield break;
    }


    private bool IsWithinRange(GameObject _object, float _minRange, float _maxRange)
    {
        Vector3 myPosition = gameObject.transform.position;
        Vector3 objectPosition = _object.transform.position;
        float distanceToObject = Vector3.Distance(myPosition, objectPosition);

        // if 3 > DTO < 5 it is in range

        if (distanceToObject < _maxRange)
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

        Debug.DrawRay(transform.position, _target.transform.position, Color.green);
        Vector3 target = new Vector3(_target.transform.position.x, transform.position.y, _target.transform.position.z);
        navAgent.SetDestination(target);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (currentAttack.attackName == "GroundPound")
            {  
                if (!player.GetComponent<PlayerMovement>().grounded)
                {
                    return;
                }
                else
                {
                    player.GetComponent<PlayerCombat>().TakeDamage(currentAttack.damage);
                    player.GetComponent<Rigidbody>().AddForce((new Vector3(0, 0.1f, 0)) * 100, ForceMode.Impulse);
                }
            }
            else if (currentAttack.attackName == "Laser")
            {
                damageOverTime = StartCoroutine(DamageOverTime());
            }
            else
            {
                player.GetComponent<PlayerCombat>().TakeDamage(currentAttack.damage);
                //temporary garbage code 
                if(currentAttack.attackName == "Slash") player.GetComponent<Rigidbody>().AddForce((transform.forward+new Vector3(0,0.02f,0)) * 300, ForceMode.Impulse);
            }
        }
    }
    private IEnumerator DamageOverTime()
    {
        while (currentAttack.attackName == "Laser")
        {
            player.GetComponent<PlayerCombat>().TakeDamage(currentAttack.damage);
            yield return new WaitForSeconds(0.8f);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(damageOverTime!= null) StopCoroutine(damageOverTime);
            damageOverTime = null;
        }
    }

    private void Hurt()
    {
        if (enteringState)
        {
            //runs once when state is entered

            animator.SetTrigger("hurt");

            StartCoroutine(Flash(0.2f));

            enteringState = false;
            //navAgent.isStopped = true;
        }
    }
    public void TakeDamage(float _damage, float _knockback)
    {
        //start damage 
        healthComponent.SubtractHealth(_damage);

        if (healthComponent.GetHealth() <= 0)
        {
            //winScreen.SetActive(true);
            EnterState("DeadState");
        }
        else
        {
            //EnterState("Hurt");
            //navAgent.Warp(transform.position - transform.forward * _knockback);

            //animator.SetTrigger("hurt");

            StartCoroutine(Flash(0.2f));

            //transform.DOShakePosition(0.2f, 0.2f, 1);
        }
    }
    Color originalColor;
    private IEnumerator Flash(float _duration)
    {

        foreach (Material mat in mats)
        {
            originalColor = colors[mats.IndexOf(mat)];
            mat.DOColor(Color.red, _duration / 2);
        }
        yield return new WaitForSeconds(_duration + _duration / 2);
        foreach (Material mat in mats)
        {
            mat.DOColor(originalColor, _duration / 2);
        }

        yield return null;
    }

    private void SaveMaterialColors()
    {
        foreach (Material mat in mats)
        {
            Color matColor = mat.color;
            colors.Add(matColor);
        }
    }
    public GameObject GetDamageableGameObject()
    {
        return gameObject;
    }

}
