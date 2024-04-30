using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("References")]
    private PlayerMovement pm;
    [HideInInspector] public Stamina staminaComponent;
    public Animator anim;
    public PlayerCam cam;
    private Health healthComponent;

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
    [HideInInspector]public float staminaRequired;

    private void Start()
    {
        pm = GetComponent<PlayerMovement>();
        staminaComponent = GetComponent<Stamina>();
        staminaComponent.SetStartingStamina(maxStamina);
        healthComponent = GetComponent<Health>();
        healthComponent.SetStartingHealth(100);
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
                StaticEventHandler.CallPlayerAttackEvent(attack1);
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
                StaticEventHandler.CallPlayerAttackEvent(attack2);
            }
        }
        else if (Input.GetButtonDown(specialAttackInput))
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
                StaticEventHandler.CallPlayerAttackEvent(specialAttack);
            }
        }

    }

    public void TakeDamage(int _damage)
    {
        healthComponent.SubtractHealth(_damage);
        if(healthComponent.GetHealth() <= 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Debug.Log("Hit");
            EnemyBehaviour enemyBehaviour = other.gameObject.GetComponent<EnemyBehaviour>();
            enemyBehaviour.TakeDamage(damage, knockback);
        }
    }

}
