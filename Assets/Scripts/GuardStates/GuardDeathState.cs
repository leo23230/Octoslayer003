using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GuardDeathState : GuardBaseState
{
    GuardStateManager guard;

    public AudioSource strangleSound;

    public TextMeshProUGUI textObject;
    public override void EnterState(GuardStateManager _guard)
    {
        Debug.Log("Entered Death State");

        _guard.animator.SetTrigger("Die");

        guard = _guard;

        //manipulate player object
        var player = guard.player;
        var playerRB = player.GetComponent<Rigidbody>();
        var playerAnimator = player.transform.Find("octo002").GetComponent<Animator>();
        playerAnimator.SetTrigger("strangle");

        strangleSound = GetComponent<AudioSource>();
        strangleSound.Play();

        //start death sequence
        StartCoroutine(StrangleDeathSequence(playerRB, player.transform.Find("octo002")));

        if(gameObject.name == "Shamu")
        {
            //guard.player.gameObject.GetComponent<PlayerQuestTrack>().hasKey = true;
            StartCoroutine(DisplayMessage("Hint: You found a key"));
        }

    }

    IEnumerator StrangleDeathSequence(Rigidbody _playerRB, Transform _playerTransform)
    {
        bool strangling = true;
        float strangleTime = 0f;
        //switch camera for epic kill shot


        //cut player gravity and momentum
        _playerRB.useGravity = false; 
        _playerRB.velocity = new Vector3(0f, 0f, 0f);

        //set player rotation for animation
        Vector3 oldRot = _playerTransform.localRotation.eulerAngles;
        Vector3 newRot = new Vector3(60, oldRot.y, oldRot.z);

        //The rotation needs to be set every frame thanks to the animator (:
        while (strangling)
        {
            _playerTransform.localRotation = Quaternion.Euler(newRot);

            Vector3 anchorPosition = transform.Find("StrangleAnchor").position;
            _playerTransform.parent.position = anchorPosition;

            strangleTime += Time.deltaTime;
            if (strangleTime >= 1.5f) strangling = false;
            yield return null;
        }

        //Reuntroduce gravity to player
        _playerRB.useGravity = true;

        //Rotate player back to normal

        _playerTransform.localRotation = Quaternion.Euler(oldRot);

        //Switch camera back to first person

        //Trigger Guard death animation
        guard.animator.SetTrigger("Die"); 
    }

    public override void UpdateState(GuardStateManager _guard)
    {

    }
    IEnumerator DisplayMessage(string message)
    {
        yield return new WaitForSeconds(2);
        textObject.text = message;
        if (textObject.enabled == false) textObject.enabled = true;
        yield return new WaitForSeconds(3);
        textObject.text = "Hint:";
        textObject.enabled = false;
    }
}
