using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Grabbed : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Rigidbody rb;
    public PlayerMovement pm;
    public PlayerCam cam;
    public GameObject pressRB;

    public bool grabbed;
    private GameObject grabTarget;

    private Coroutine breakFreeCoroutine;
    

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        StateMachine();
    }
    private void StateMachine()
    {
        //State 1 - climbing
        if (grabbed)
        {
            GrabbedMovement();
        }
        else
        {

        }
    }
    private void GrabbedMovement()
    {
        Vector3 newPosition = new Vector3(grabTarget.transform.position.x, transform.position.y, grabTarget.transform.position.z);
        rb.MovePosition(newPosition);
    }
    public void GrabPlayer(GameObject _grabTarget)
    {
        grabbed = true;
        pm.grabbed = true;
        pm.state = PlayerMovement.MovementState.grabbed;
        grabTarget = _grabTarget;
        breakFreeCoroutine = StartCoroutine(BreakFree());
    }
    public void ReleasePlayer()
    {
        grabbed = false;
        pm.grabbed = false;
        pm.state = PlayerMovement.MovementState.walking;
        grabTarget = null;
        StopCoroutine(breakFreeCoroutine);
        pressRB.SetActive(false);
    }

    private IEnumerator BreakFree()
    {
        float fill = 0;
        float max = 1;

        //Give it some time
        yield return new WaitForSeconds(1.5f);

        pressRB.SetActive(true);
        while (fill < max)
        {
            if(fill >= 0)
            {
                fill -= 0.006f;
            }
            if (Input.GetButtonDown("Special"))
            {
                fill += 0.20f;
            }
            Debug.Log("Fill " + fill);
            yield return null;
        }
        pressRB.SetActive(false);

        //call event so boss can react to player grab cancel
        StaticEventHandler.CallGrabCancelEvent();

        ReleasePlayer();

        yield break;
    }
}
