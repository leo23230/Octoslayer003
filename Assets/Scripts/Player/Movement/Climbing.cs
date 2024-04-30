using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbing : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Rigidbody rb;
    public PlayerMovement pm;
    public PlayerCam cam;
    public LayerMask whatIsWall;

    [Header("Climbing")]
    public float climbSpeed;
    public float maxClimbTime;
    private float climbTimer;

    private bool climbing;

    [Header("Detection")]
    public float detectionLength;
    public float sphereCastRadius;
    public float maxWallLookAngle;
    private float wallLookAngle;

    private RaycastHit frontWallHit;
    private bool wallFront;

    private void Update()
    {
        WallCheck();
        StateMachine();

        if (climbing) ClimbingMovement();
    }

    private void StateMachine()
    {
        //State 1 - climbing
        if(wallFront && Input.GetAxisRaw("Vertical") > 0 && wallLookAngle < maxWallLookAngle)
        {
            if (!climbing && climbTimer > 0) StartClimbing();

            //timer
            if (climbTimer > 0) climbTimer -= Time.deltaTime;
            if (climbTimer < 0) StopClimbing();
        }
        else
        {
            if (climbing) StopClimbing();
        }
    }

    private void WallCheck()
    {
        wallFront = Physics.SphereCast(transform.position, sphereCastRadius, orientation.forward, out frontWallHit, detectionLength, whatIsWall);
        //delegates a range of acceptable angels to begin a wall climb as opposed to a wall run or jump
        wallLookAngle = Vector3.Angle(orientation.forward, -frontWallHit.normal);

        if (pm.grounded)
        {
            climbTimer = maxClimbTime;
        }
    }

    private void StartClimbing()
    {
        climbing = true;
        pm.climbing = true;

        //camera effects
        cam.DoZoomOut(3f);
        cam.DoFov(90f);

        //rotate player
        pm.DoPlayerObjectRotate(new Vector3(-90f, 0f, 0f));
        //pm.DoPlayerObjectMove(new Vector3(0f,0f,-transform.forward.z * pm.playerHeight/2));

        //play sound
    }

    private void ClimbingMovement()
    {
        //easier to set y velocity directly instead of adding force
        rb.velocity = new Vector3(rb.velocity.x, climbSpeed, rb.velocity.z);
    }

    private void StopClimbing()
    {
        climbing = false;
        pm.climbing = false;

        //camera effects
        cam.DoZoomOut(0);
        cam.DoFov(80f);

        //reset player rotation
        pm.DoPlayerObjectRotate(new Vector3(0f, 0f, 0f));
        //pm.DoPlayerObjectMove(new Vector3(0f, 0f, 0f));
    }
}
