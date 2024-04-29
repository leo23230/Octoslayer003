using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
{
    public GameObject shield;
    public FadeIn fadeComponent;
    private Animator anim;
    public EnemyBehaviour enemyBehaviour;

    //variables//
    private float shieldPauseTimerDefault = 0.3f;
    private float shieldPauseTimer = 0.3f;
    

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        StaticEventHandler.OnPlayerAttack += AddTimeToShieldPause;
    }

    private void OnDisable()
    {
        StaticEventHandler.OnPlayerAttack -= AddTimeToShieldPause;
    }

    public void FadeInShield()
    {
        //shield.GetComponent<MeshRenderer>().enabled = true;
        //shield.GetComponent<SphereCollider>().isTrigger = false;
        shield.SetActive(true);
    }
    public void FadeOutShield()
    {
        StartCoroutine(FadeOut());
    }
    private IEnumerator FadeOut()
    {
        fadeComponent.alpha = 0f;
        while (fadeComponent.alpha > -0.8f)
        {
            fadeComponent.alpha -= 0.1f;
            if (fadeComponent.alpha > 0) fadeComponent.alpha = Mathf.Ceil(0);

            fadeComponent.mesh.material.SetTextureOffset("_MainTex", new Vector2(fadeComponent.alpha, 0f));

            yield return null;
        }
        //shield.GetComponent<MeshRenderer>().enabled = false;
        //shield.GetComponent<SphereCollider>().isTrigger = true;
        shield.SetActive(false);
        yield return null;
    }

    public void PauseShieldAnimation()
    {
        //this is always triggered when the shield is fully extended
        //the default is zero seconds, for every hit event recieved, the time is increased
        if(shieldPauseTimer > 0)
        {
            StartCoroutine(TimedShieldPause());
        }
    }

    IEnumerator TimedShieldPause()
    {
        anim.speed = 0;
        while(shieldPauseTimer > 0)
        {
            shieldPauseTimer -= Time.deltaTime;
            yield return null;
        }
        anim.speed = 1;
        //reset pause timer
        shieldPauseTimer = shieldPauseTimerDefault;
    }

    public void ResetBlockBool()
    {
        anim.SetBool("block", false);
        enemyBehaviour.SwitchState("Combat");
    }

    public void AddTimeToShieldPause(PlayerAttackEventArgs eventArgs)
    {
        //subscribed to player attack event
        if (!eventArgs.attack.isSpecial)
        {
            if(anim.GetBool("block"))
            {
                if(shieldPauseTimer < 3f)
                {
                    shieldPauseTimer += 1f;
                }
            }
        }
    }
}
