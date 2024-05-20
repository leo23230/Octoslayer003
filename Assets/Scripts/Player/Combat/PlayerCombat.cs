using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;
using PixelCrushers.DialogueSystem;

public class PlayerCombat : MonoBehaviour
{
    public bool DeveloperMode;
    [Header("References")]
    private PlayerMovement pm;
    [HideInInspector] public Stamina staminaComponent;
    public Animator anim;
    public PlayerCam cam;
    private Health healthComponent;
    public VolumeProfile volume;
    private ColorAdjustments colorAdjustments;
    public ItemSO freeBossKey;
    public ItemSO poison;
    public ItemSO freePassword;

    [Header("Stats")]
    public int maxStamina = 100;

    [Header("Keybinds")]
    public string primaryAttackInput = "PrimaryAttack";
    public string heavyAttackInput = "HeavyAttack";
    public string specialAttackInput = "Special";
    public string grab = "Grab";

    [Header("Player Attacks")]
    public AttackDetailsSO attack1;
    public AttackDetailsSO attack2;
    public AttackDetailsSO specialAttack;
    public List<AttackDetailsSO> unlockedAttacks;

    private float damage;
    private float knockback;
    private List<IDamageable> markedForDamage = new List<IDamageable>();

    //remember to reset in animation events
    [HideInInspector]public bool immune;
    [HideInInspector]public float staminaRequired;

    //misc
    [ColorUsage(true,true)]
    Color newColor;

    private void Start()
    {
        pm = GetComponent<PlayerMovement>();
        staminaComponent = GetComponent<Stamina>();
        staminaComponent.SetStartingStamina(maxStamina);
        healthComponent = GetComponent<Health>();
        healthComponent.SetStartingHealth(100);

        volume.TryGet(out colorAdjustments);
        colorAdjustments.colorFilter.hdr = true;
    }

    //first we want to send an event that the player is attacking

    //enemies will recieve this event, and based on their state, block the attack or be vulnerable to it

    //the player will perform the animations, enemies will be marked for damage

    //if the enemy is hit by the collider and vulnerable to the attack, they will take damage.

    //the enemy will have a countdown timer of time that they are vulnerable before and after an attack

    //that countdown timer will be equal to the exit time / transition from the enemy attack animation to the block
    //during that time they will be in a vulnerable state
    //by sending an event, we are being more forgiving to the player. This way they don't have to time the animation of their attack.

    private void Update()
    {
        //DEV CHEAT MUST DELETE//
        if (DeveloperMode)
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                Inventory.instance.Add(freeBossKey);
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                Inventory.instance.Add(freePassword);
            }
        }

        if (Input.GetButtonDown(primaryAttackInput))
        {
            //set damage and knockback
            damage = attack1.damage;
            knockback = attack1.knockback;
            staminaRequired = attack1.staminaRequired;

            //switch movement state

            bool staminaRequirmentMet = staminaComponent.GetStamina() >= staminaRequired;
            if (staminaRequirmentMet)
            {

                //animation
                anim.SetTrigger(attack1.animTrigger);
                //staminaComponent.SubtractStamina(staminaRequired);

                //call event
                if(!PlayerStealth.instance.bossIsActive) StaticEventHandler.CallPlayerAttackEvent(attack1);
            }
        }
        else if (Input.GetButtonDown(heavyAttackInput))
        {
            damage = attack2.damage;
            knockback = attack2.knockback;
            staminaRequired = attack2.staminaRequired;

            bool staminaRequirmentMet = staminaComponent.GetStamina() >= staminaRequired;
            if (staminaRequirmentMet)
            {

                //animation
                anim.SetTrigger(attack2.animTrigger);
                //staminaComponent.SubtractStamina(staminaRequired);

                //call event
                if (!PlayerStealth.instance.bossIsActive) StaticEventHandler.CallPlayerAttackEvent(attack2);
            }
        }
        else if (Input.GetButtonDown(specialAttackInput) && pm.grounded && pm.state != PlayerMovement.MovementState.grabbed)
        {
            damage = specialAttack.damage;
            knockback = specialAttack.knockback;
            staminaRequired = specialAttack.staminaRequired;

            //switch movement state

            bool staminaRequirmentMet = staminaComponent.GetStamina() >= staminaRequired;
            if (staminaRequirmentMet)
            {

                //animation
                anim.SetTrigger(specialAttack.animTrigger);
                //staminaComponent.SubtractStamina(staminaRequired);

                //call event
                if (!PlayerStealth.instance.bossIsActive) StaticEventHandler.CallPlayerAttackEvent(specialAttack);

                //always immune during special attacks
                immune = true;
            }
        }

    }

    public void TakeDamage(int _damage)
    {
        Debug.Log("Player Hit");
        UIEffects.instance.RedFlash(0.25f);
        if (!immune)
        {
            healthComponent.SubtractHealth(_damage);
            if (healthComponent.GetHealth() <= 0)
            {
                DialogueManager.ResetDatabase();
                if (UnityEngine.SceneManagement.SceneManager.GetActiveScene() == UnityEngine.SceneManagement.SceneManager.GetSceneByBuildIndex(0)) UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            }
        }
    }

    public void DoDamageToMarkedTargets()
    {
        foreach(IDamageable damageableObject in markedForDamage)
        {
            damageableObject.TakeDamage(damage, knockback);

            //posion
            if (damageableObject.GetDamageableGameObject().CompareTag("Del"))
            {
                if (Inventory.instance.Contains(poison))
                {
                    StartCoroutine(DamageOverTime(damageableObject, 5, 3, 1));
                }
            }
        }
        markedForDamage.Clear();     
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IDamageable>()!=null)
        {
            IDamageable damageableObject = other.GetComponent<IDamageable>();
            markedForDamage.Add(damageableObject);
        }
        else if (other.name == "TootsBody")
        {
            //temporary, will be replaced with npc sate machine
            Toots NPCStateMachine = other.transform.parent.GetComponent<Toots>();
            NPCStateMachine.EnterState("DeathState");
        }
        else if (other.name == "FlukeBody")
        {
            Fluke NPCStateMachine = other.transform.parent.GetComponent<Fluke>();
            NPCStateMachine.EnterState("DeathState");
        }
    }
    
    private IEnumerator DamageOverTime(IDamageable damageableObject, float damage, float duration, float frequency)
    {
        float timer = duration;
        while(timer > 0)
        {
            yield return new WaitForSeconds(frequency);
            damageableObject.TakeDamage(damage, knockback);
            timer -= frequency;
        }
        yield break;
    }

}
