using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DelBossAnimationEvents : MonoBehaviour
{
    public DelBossSM sm;
    private Animator anim;

    [Header("Effects")]
    public GameObject GroundPoundEffect;
    public GameObject laserPrefab;
    public GameObject laserAnchor;
    public GameObject laserHitbox;

    [Header("References")]
    public PlayerCam cam;
    public List<Collider> attackHitboxes = new List<Collider>();
    public GameObject grabTarget;

    private void Start()
    {
        anim = GetComponent<Animator>();

        foreach(Collider collider in attackHitboxes)
        {
            collider.gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        StaticEventHandler.OnGrabCancel += CancelGrab;
    }
    private void OnDisable()
    {
        StaticEventHandler.OnGrabCancel -= CancelGrab;
    }

    public void Step()
    {
        //play sound
        float distToPlayer = Vector3.Distance(transform.position, sm.player.transform.position);
        cam.DoCamShake(0.1f, 0.2f/distToPlayer);
    }
    public void GroundPoundEffects()
    {
        GameObject effect = Instantiate(GroundPoundEffect);
        effect.transform.position = new Vector3(transform.position.x, transform.position.y+0.1f, transform.position.z);
        effect.transform.parent = transform.parent;
        cam.DoCamShake(0.2f, 1);
    }
    public void ResetGroundPount()
    {
        anim.SetBool("groundPound", false);
    }

    public void ResetSlash()
    {
        anim.SetBool("slash", false);
    }
    public void StartLaser()
    {
        StartCoroutine(FireLaser());
    }
    private IEnumerator FireLaser()
    {
        GameObject laser = Instantiate(laserPrefab);
        laser.transform.position = laserAnchor.transform.position;
        laser.transform.parent = transform;
        laser.transform.rotation = transform.rotation;
        anim.speed = 0;
        yield return new WaitForSeconds(2);
        laserHitbox.SetActive(true);
        yield return new WaitForSeconds(4);

        Destroy(laser); //Delete Effect
        anim.speed = 1;
        anim.SetBool("laser", false);
        laserHitbox.SetActive(false);

        sm.currentAttack = null;

        yield break;
    }
    public void ResetTackle()
    {
        anim.SetBool("tackle", false);
    }

    public void GrabPlayer()
    {
        float distToPlayer = Vector3.Distance(transform.position, sm.player.transform.position);
        if (distToPlayer < 5f)
        {
            sm.player.GetComponent<Grabbed>().GrabPlayer(grabTarget);
        }
    }
    public void DamagePlayer()
    {
        if (sm.player.GetComponent<Grabbed>().grabbed) sm.player.GetComponent<PlayerCombat>().TakeDamage(3);
    }
    public void ReleasePlayer()
    {
        if(sm.player.GetComponent<Grabbed>().grabbed) sm.player.GetComponent<Grabbed>().ReleasePlayer();
    }
    public void CancelGrabIfNoPlayer()
    {
        if (!sm.player.GetComponent<Grabbed>().grabbed) ResetTackle();
    }
    public void CancelGrab(GrabCancelEventArgs eventArgs)
    {
        anim.SetTrigger("stun");
    }
    public void GrabMoveForward()
    {
        StartCoroutine(GrabMoveSequence());
    }
    IEnumerator GrabMoveSequence()
    {
        //sm.rb.velocity = sm.transform.forward;
        //transform.DOLocalMove(sm.transform.forward, 1f);
        //sm.navAgent.Move(sm.transform.forward);
        yield return new WaitForSeconds(2f);
        //sm.rb.velocity = Vector3.zero;
        
    }
}
