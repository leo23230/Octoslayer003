using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "_AttackDetails", menuName = "Scriptables/Player/AttackDetails")]
public class AttackDetailsSO : ScriptableObject
{
    [Header("Basic Info")]
    public string attackName;
    public string description;
    public string animTrigger;

    [Header("Stats")]
    public float damage;
    public float speed;

    [Header("Type")]
    public bool isLight;
    public bool isHeavy;
    public bool isSpecial;
}
