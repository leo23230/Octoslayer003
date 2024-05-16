using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    public List<ItemSO> items = new List<ItemSO>();

    public bool Contains(ItemSO _item)
    {
        if (items.Contains(_item))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void Add(ItemSO _item)
    {
        items.Add(_item);
    }
}
