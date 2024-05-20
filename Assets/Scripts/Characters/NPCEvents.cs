using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCEvents : MonoBehaviour
{
    public GameObject fluke;
    public DelBossTrigger delBossTrigger;
    public ItemSO password;

    private void Start()
    {
        fluke.SetActive(false);
    }

    public void EnableFluke()
    {
        fluke.SetActive(true);
    }

    //called if player listens to him and takes his offer.
    public void DisableFluke()
    {
        UIEffects.instance.FadeScreen(0.4f);
        Inventory.instance.Add(password);
        StartCoroutine(DelayedDisableFluke(0.4f));
    }
    IEnumerator DelayedDisableFluke(float _duration)
    {
        yield return new WaitForSeconds(_duration);
        fluke.SetActive(false);
        yield break;
    }
    public void StartDelBossFight()
    {
        delBossTrigger.StartBossFight();
    }
    public void CloseDelDoor()
    {
        delBossTrigger.CloseDelDoor();
    }
}
