using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="_Item",menuName = "Scriptables/Items,Weapons,Consumables/Item")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public bool isConsumable;
    public bool isKey;
}
