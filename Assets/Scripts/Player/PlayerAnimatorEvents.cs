using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorEvents : MonoBehaviour
{
    public PlayerCam cam;
    public PlayerMovement pm;

    private void Start()
    {
        
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
}
