using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunning : MonoBehaviour
{
    [Header("Wallrunning")]
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    public float wallRunForce;
    public float wallJumpUpForce;
    public float wallJumpSideForce;
    public float wallClimbSpeed;
    public float maxWallRunTime;
    private float wallRunTimer;

    [Header("Input")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode upwardsRunKey = KeyCode.LeftShift;
    public KeyCode downwardsRunKey = KeyCode.LeftControl;
    private bool upwardsRunning;
    private bool downwardsRunning;
    private float horizontalInput;
    private float verticalInput;

    [Header("Detection")]
    public float wallCheckDistance;
    public float minJumpHeight;
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    private bool wallLeft;
    private bool wallRight;

    [Header("Exiting")]
    private bool exitingWall;
    public float exitWallTime;
    private float exitWallTimer;

    [Header("References")]
    public Transform orientation;
    public PlayerCam cam;
    private PlayerMovement pm;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        CheckForWall();
        StateMachine();
    }

    private void FixedUpdate()
    {
        if (pm.wallrunning)
            WallRunningMovement();
    }

    private void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallCheckDistance, whatIsWall);
    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }

    private void StateMachine()
    {
        // Getting Inputs
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        upwardsRunning = Input.GetKey(upwardsRunKey);
        downwardsRunning = Input.GetKey(downwardsRunKey);

        // State 1 - Wallrunning
        if ((wallLeft || wallRight) && verticalInput > 0 && AboveGround() && !exitingWall)
        {
            if (!pm.wallrunning)
                StartWallRun();

            //wall jump
            if (Input.GetKeyDown(jumpKey))
                WallJump();
        }

        //state 2
        else if (exitingWall)
        {
            if (pm.wallrunning)
                StopWallRun();
            if (exitWallTimer > 0)
                exitWallTimer -= Time.deltaTime;
            if (exitWallTimer <= 0)
                exitingWall = false;
        }

        // State 3 - None
        else
        {
            if (pm.wallrunning)
                StopWallRun();
        }
    }

    private void StartWallRun()
    {
        pm.wallrunning = true;

        //apply camera effects
        cam.DoFov(90f);
        if (wallLeft) cam.DoTilt(-5f);
        if (wallRight) cam.DoTilt(5f);
    }

    private void WallRunningMovement()
    {
        rb.useGravity = false;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;

        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if ((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
            wallForward = -wallForward;

        // forward force
        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);

        // upwards force
        //set y velocity of rigid body to wall climb speed
        if (upwardsRunning)
            rb.velocity = new Vector3(rb.velocity.x, wallClimbSpeed, rb.velocity.z);
        if (downwardsRunning)
            rb.velocity = new Vector3(rb.velocity.x, -wallClimbSpeed, rb.velocity.z);


        // push to wall force
        if (!(wallLeft && horizontalInput > 0) && !(wallRight && horizontalInput < 0))
            rb.AddForce(-wallNormal * 100, ForceMode.Force);
    }

    private void StopWallRun()
    {
        rb.useGravity = true;
        pm.wallrunning = false;

        //reset camera effects
        cam.DoFov(80f);
        cam.DoTilt(0f);
    }

    private void WallJump()
    {
        //enter exiting wall state
        exitingWall = true;
        exitWallTimer = exitWallTime;

        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;

        //multiply upwards direction with walljumpup force and normal with wall jump side force
        Vector3 forceToApply = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;

        //add force
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);
    }
}

/*
private void FixedUpdate()
{
    if (pm.wallrunning)
        WallRunningMovement();
}

private void Update()
{
    CheckForWall();
    StateMachine();
}

private void CheckForWall()
{
    wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallCheckDistance, whatIsWall);
    wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallCheckDistance, whatIsWall);
}

private bool AboveGround()
{
    //return true if ray hits nothing
    return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
}

private void StateMachine()
{
    //Getting Inputs
    horizontalInput = Input.GetAxisRaw("Horizontal");
    verticalInput = Input.GetAxisRaw("Vertical");

    //State 1 - Wallrunning
    if((wallLeft || wallRight) && verticalInput > 0 && AboveGround())
    {
        //start wallrun here
        if (!pm.wallrunning)
            StartWallRun();
    }
    else
    {
        if (pm.wallrunning)
            StopWallRun();
    }
}

private void StartWallRun()
{
    pm.wallrunning = true;
}

private void WallRunningMovement()
{
    Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;

    Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

    //calculate which direction is closest to the direction the player is facing.
    if ((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
        wallForward = -wallForward;

    //forward force
    rb.AddForce(wallForward * wallRunForce, ForceMode.Force);

    //push player to wall force
    //only want to add this if the player is not trying to get away from the wall
    if(!(wallLeft && horizontalInput > 0) && !(wallRight && horizontalInput < 0))
        rb.AddForce(-wallNormal * 100, ForceMode.Force);
}

private void StopWallRun()
{
    rb.useGravity = true;
    pm.wallrunning = false;
}
}*/
