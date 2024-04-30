using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    private AudioSource source;
    public AudioClip slashSound;
    public AudioClip stabSound;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip _clip)
    {
        source.clip = _clip;
        source.Play();
    }
}
