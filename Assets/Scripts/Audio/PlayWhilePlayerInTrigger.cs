using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayWhilePlayerInTrigger : MonoBehaviour
{
    private AudioSource audioSource;
    private float audioStartingVolume;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioStartingVolume = audioSource.volume;
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ENTERING OCEAN ZONE");
        if (other.CompareTag("Player"))
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
                audioSource.volume = audioStartingVolume;
            }
        }
    }
   /* private void OnTriggerStay(Collider other)
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
            audioSource.volume = audioStartingVolume;
        }
    }*/
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Left Ocean Zone");
        if (other.CompareTag("Player"))
        {
            Debug.Log("PLAYER Left Ocean Zone");
            StartCoroutine(FadeOut());
        }
    }
    private IEnumerator FadeOut()
    {
        while(audioSource.volume > 0)
        {
            Debug.Log("FADING OUT");
            audioSource.volume -= 0.01f;
            yield return null;
        }
        audioSource.Stop();
        yield break;
    }
}
