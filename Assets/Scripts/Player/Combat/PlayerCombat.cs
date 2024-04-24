using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("References")]
    private PlayerMovement pm;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Player Attacks")]
    public AttackDetailsSO attack1;
    public AttackDetailsSO attack2;
    public AttackDetailsSO specialAttack;
    public List<AttackDetailsSO> unlockedAttacks;

    private void Start()
    {
        pm = GetComponent<PlayerMovement>();
    }
}
