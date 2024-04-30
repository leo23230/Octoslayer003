using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class PlayerAudioEvents : MonoBehaviour
{
    [EventRef, SerializeField] string slashSound = default;
    [EventRef, SerializeField] string stabSound = default;

    FMOD.Studio.EventInstance slashEventInstance;
    FMOD.Studio.EventInstance stabEventInstance;


    void Start()
    {
        slashEventInstance = RuntimeManager.CreateInstance(slashSound);
        stabEventInstance = RuntimeManager.CreateInstance(stabSound);
    }
}
