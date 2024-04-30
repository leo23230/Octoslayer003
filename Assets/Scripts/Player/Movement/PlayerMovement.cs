using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;
    public float wallrunSpeed;
    public float climbSpeed;

    [Header("FightingMovement")]
    public float blockingSpeed;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;

    [Header("References")]
    public Transform orientation;
    public Transform playerObject;
    public Animator anim;

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
        climbing,
        slashing,
        stabbing,
        grabbingEnemy,
        blocking,
        crounching,
        sliding,
        air
    }
    public bool sliding;
    public bool crouchin;
    public bool wallrunning;
    public bool climbing;
    public bool slashing;
    public bool grabbingEnemy;
    public bool stabbing;
    public bool blocking;

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
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //when to jump
        if(Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void GroundCheck()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
    }

    private void HandleDrag()
    {
        if (grounded)
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

    private void StateHandler()
    {
        //Mode - Climbing
        if (climbing)
        {
            state = MovementState.climbing;
            moveSpeed = climbSpeed;
        }

        //Mode - Wallrunning
        else if (wallrunning)
        {
            state = MovementState.wallrunning;
            moveSpeed = wallrunSpeed;
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
}
