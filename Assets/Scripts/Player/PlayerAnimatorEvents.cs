using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;
using FMODUnity;

public class PlayerAnimatorEvents : MonoBehaviour
{
    public PlayerCam cam;
    public PlayerMovement pm;
    public PlayerCombat pc;
    private PlayerAudioManager playerAudio;

    private void Start()
    {
        playerAudio = GetComponent<PlayerAudioManager>();
    }
    public void CameraZoomOut()
    {
        pm.Jump();
        cam.DoZoomOut(4);
        cam.DoFov(80f);     
    }

    public void CameraShake()
    {
        cam.DoCamShake(0.2f, 0.2f);
    }

    public void CameraReset()
    {
        cam.DoZoomOut(0);
        cam.DoFov(60f);
    }

    public void SubtractStamina()
    {
        pc.staminaComponent.SubtractStamina(pc.staminaRequired);
    }

    public void PlaySlashSound()
    {
        playerAudio.PlaySound(playerAudio.slashSound);
    }
    public void PlayStabSound()
    {
        playerAudio.PlaySound(playerAudio.stabSound);
    }
}
