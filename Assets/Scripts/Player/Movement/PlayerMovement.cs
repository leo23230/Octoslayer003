using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float walkSpeed;
    public float groundDrag;
    public float wallrunSpeed;
    public float climbSpeed;
    public float dashSpeed;
    public float dashSpeedChangeFactor;

    [Header("FightingMovement")]
    public float blockingSpeed;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Keybinds")]
    public string jumpKey = "Jump";

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;

    [Header("References")]
    public Transform orientation;
    public Transform playerObject;
    public Animator anim;
    public AudioSource movementAudioSource;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    Rigidbody rb;

    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        wallrunning,
        dashing,
        climbing,
        slashing,
        stabbing,
        grabbingEnemy,
        blocking,
        crounching,
        sliding,
        air,
        grabbed,
    }
    public bool sliding;
    public bool dashing;
    public bool crouchin;
    public bool wallrunning;
    public bool climbing;
    public bool slashing;
    public bool grabbingEnemy;
    public bool stabbing;
    public bool blocking;
    public bool grabbed;

    public bool climbPossible;

    void Start()
    {
        readyToJump = true;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        MovePlayer();
        HandleDrag();
    }

    private void Update()
    {
        Cursor.visible = false;
        GroundCheck();
        MyInput();
        SpeedControl();
        StateHandler();
        PlayWalkingSound();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //when to jump
        if(Input.GetButtonDown(jumpKey) && readyToJump && grounded && !climbPossible)
        {
            if(state != MovementState.grabbed)
            {
                readyToJump = false;

                Jump();

                Invoke(nameof(ResetJump), jumpCooldown);
            } 
        }
    }

    private void GroundCheck()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
    }

    private void HandleDrag()
    {
        if (grounded && !dashing)
            rb.drag = groundDrag;
        else
            rb.drag = 0f;
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //limit velocity if needed
        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void MovePlayer()
    {
        //animation
        Vector2 speed = new Vector2(horizontalInput, verticalInput);
        anim.SetFloat("speed", speed.sqrMagnitude);

        //calculate movement direction

        //always walk in direction you are looking
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //onground
        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f*airMultiplier, ForceMode.Force);
    }

    public void Jump()
    {
        //reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //impulse force for applying a force once
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;
    private MovementState lastState;
    private bool keepMomentum;
    private void StateHandler()
    {
        if (dashing)
        {
            state = MovementState.dashing;
            desiredMoveSpeed = dashSpeed;
            speedChangeFactor = dashSpeedChangeFactor;
        }
        //Mode - Climbing
        else if (climbing)
        {
            state = MovementState.climbing;
            desiredMoveSpeed = climbSpeed;
        }

        //Mode - Wallrunning
        else if (wallrunning)
        {
            state = MovementState.wallrunning;
            desiredMoveSpeed = wallrunSpeed;
        }

        //COMBAT//

        //Mode - slashing
        else if (slashing)
        {
            state = MovementState.slashing;
        }

        //Mode - stabbing
        else if (stabbing)
        {
            state = MovementState.stabbing;
        }

        else if (grabbingEnemy)
        {
            state = MovementState.grabbingEnemy;
        }

        //Mode - blocking
        else if (blocking)
        {
            state = MovementState.blocking;
        }
        else if (grabbed)
        {
            state = MovementState.grabbed;
            //moveSpeed = 0f;
        }
        else if(grounded)
        {
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
        }
        else
        {
            state = MovementState.air;
        }

        bool desiredMoveSpeedHasChanged = desiredMoveSpeed != lastDesiredMoveSpeed;
        if (lastState == MovementState.dashing) keepMomentum = true;

        if (desiredMoveSpeedHasChanged)
        {
            if (keepMomentum)
            {
                StopAllCoroutines();
                StartCoroutine(SmoothlyLerpMoveSpeed());
            }
            else
            {
                moveSpeed = desiredMoveSpeed;
            }
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;
    }
    private float speedChangeFactor;
    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        // smoothly lerp movementSpeed to desired value
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        float boostFactor = speedChangeFactor;

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);

            time += Time.deltaTime * boostFactor;

            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
        speedChangeFactor = 1f;
        keepMomentum = false;
    }

    public void DoPlayerObjectRotate(Vector3 endValue)
    {
        playerObject.DOLocalRotate(endValue, 0.3f);
    }

    public void DoPlayerObjectTransformUp(float endValue)
    {
        playerObject.DOLocalMoveY(endValue, 0.3f);
    }

    public void DoPlayerObjectMove(Vector3 endValue)
    {
        playerObject.DOLocalMove(endValue, 0.3f);
    }

    public void PlayWalkingSound()
    {
        if((Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) && (grounded||wallrunning||climbing))
        {
            if (!movementAudioSource.isPlaying)
            {
                movementAudioSource.time = Random.Range(0f, movementAudioSource.clip.length);
                movementAudioSource.Play();
            }
        }
        else
        {
            movementAudioSource.Stop();
        }
    }
}
