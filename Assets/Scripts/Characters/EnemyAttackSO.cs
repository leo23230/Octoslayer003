using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "_EnemyAttack", menuName = "Scriptables/Player/EnemyAttack")]
public class EnemyAttackSO : ScriptableObject
{
    [Header("Basic Info")]
    public string attackName;
    public string description;
    public string animTrigger;

    [Header("Stats")]
    public int damage;
    public float knockback;

    [Header("Type")]
    public bool isLight;
    public bool isHeavy;

    [Header("Duration")]
    public bool endsWithAnimation;

    [Header("IF DOES NOT END WITH ANIMATION")]
    public float duration;
}
