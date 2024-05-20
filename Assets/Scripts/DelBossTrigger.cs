using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelBossTrigger : MonoBehaviour
{
    public GameObject door;
    public MusicManager musicManager;
    public GameObject DelBoss;

    private void Start()
    {
        DelBoss.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

        }
    }
    public void CloseDelDoor()
    {
        door.SetActive(true);
    }
    public void StartBossFight()
    {
        PlayerStealth.instance.bossIsActive = true;
        musicManager.SwitchMusic(musicManager.bossFightMusic);
        StartCoroutine(EnableBossDel());
    }
    IEnumerator EnableBossDel()
    {
        yield return new WaitForSeconds(3);
        DelBoss.SetActive(true);
        gameObject.SetActive(false);
        yield break;
    }
}
