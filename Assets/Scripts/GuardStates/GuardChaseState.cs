using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class GuardChaseState : GuardBaseState
{
    public float speed = 5;
    public GameObject capturedTextObject;
    public GameObject stressEffect;

    private float slowTimeTimer;
    private float timerMax = 2f;
    private bool captured;
    public override void EnterState(GuardStateManager _guard)
    {
        captured = false;
        //Handle Animation
        _guard.animator.SetTrigger("Chase");
        transform.LookAt(_guard.player.position);

        //turn on stress animation
        stressEffect.SetActive(true);
    }

    public override void UpdateState(GuardStateManager _guard)
    {
        float distanceToPlayer = Vector3.Distance(transform.position, _guard.player.position);

        if(distanceToPlayer > 4 || !captured)
        {
            //Move Towards Player

            NavMeshAgent navAgent = GetComponent<NavMeshAgent>();

            transform.LookAt(_guard.player.position);
            Vector3 target = new Vector3(_guard.player.position.x, transform.position.y, _guard.player.position.z);
            navAgent.SetDestination(target);
            //transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

            StartCoroutine(DelayedEnd());
            capturedTextObject.SetActive(true);
            captured = true;
        }
        else
        {
            //Dive
            _guard.animator.SetTrigger("Dive");
            stressEffect.SetActive(false);
            //Then end the game

            if(slowTimeTimer < timerMax)
            {
                slowTimeTimer += Time.deltaTime;
                float percentComplete = slowTimeTimer / timerMax;
                Time.timeScale = 1 - Mathf.Lerp(0, 1, percentComplete);
            }
        }
        
    }

    IEnumerator DelayedEnd()
    {
        yield return new WaitForSeconds(2);
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
