using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "_EntryPointDetails", menuName = "Scriptables/LevelBuilding/EntryPoint")]
public class EntryPointDetailsSO : ScriptableObject
{
    [Header("General Details")]

    [Tooltip("Player's base stealth score upon entry out of 100.")]
    public int stealthScore;
    public string explanation;

    [Header("Teleport Settings")]
    public bool isTeleport;
}
