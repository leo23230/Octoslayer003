using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FadeIn : MonoBehaviour
{
    public MeshRenderer mesh;
    private Color targetColor;
    private Color startingColor;
    public float alpha;

    private void Start()
    {
        //mesh = GetComponent<MeshRenderer>();
        /*targetColor = new Color(0, 252, 255, 1);
        Debug.Log(targetColor);
        startingColor = new Color(targetColor.r, targetColor.g, targetColor.b, 0);
        mesh.material.SetColor("_TintColor", startingColor);*/

        gameObject.SetActive(false);
        //GetComponent<MeshRenderer>().enabled = false;
        //GetComponent<SphereCollider>().isTrigger = true;
    }
    private void OnEnable()
    {
        alpha = -0.8f;
        StartCoroutine(FadeAlpha());
        /*alpha = 0;
        DOTween.To(() => alpha, x => alpha = x, 255f, 0.2f);
        var tweenColor = new Color(targetColor.r, targetColor.g, targetColor.b, alpha);
        mesh.material.SetColor("_TintColor", tweenColor);*/
    }

    public IEnumerator FadeAlpha()
    {
        while(alpha < 0)
        {
            alpha += 0.1f;
            if(alpha > 0) alpha = Mathf.Ceil(0);

            mesh.material.SetTextureOffset("_MainTex", new Vector2(alpha, 0f));

            yield return null;
        }
        yield return null;
    }

    public void FadeOutCode()
    {
        alpha -= 0.1f;
        if (alpha < -0.8) alpha = Mathf.Floor(-0.8f);

        mesh.material.SetTextureOffset("_MainTex", new Vector2(alpha, 0f));
    }
    
}
