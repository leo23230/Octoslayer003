using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("References")]
    private PlayerMovement pm;
    public Animator anim;
    public PlayerCam cam;

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

    private void Start()
    {
        pm = GetComponent<PlayerMovement>();
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
            //switch movement state

            //animation
            anim.SetTrigger(attack1.animTrigger);

            //call event to notify enemies of an attack
            StaticEventHandler.CallPlayerAttackEvent(attack1);
        }
        else if (Input.GetButtonDown(heavyAttackInput))
        {
            //animation
            anim.SetTrigger(attack2.animTrigger);

            //call event
            StaticEventHandler.CallPlayerAttackEvent(attack2);
        }
        else if (Input.GetButtonDown(specialAttackInput))
        {
            //switch movement state

            //animation
            anim.SetTrigger(specialAttack.animTrigger);

            //want to give certain enemies the option to be immune to special attack stuns
            StaticEventHandler.CallPlayerAttackEvent(specialAttack);
        }

    }

}
