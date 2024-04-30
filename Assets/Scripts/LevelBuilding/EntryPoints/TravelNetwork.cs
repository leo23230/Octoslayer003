using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelNetwork : MonoBehaviour
{
    [Header("Details")]
    public string networkName;

    public List<Transform> entryPoints = new List<Transform>();
    private int currentPoint;
    private int numberOfPoints;
    private GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
        //entryPoints = new List<Transform>(transform.GetComponentsInChildren<Transform>());
        currentPoint = 0;
        numberOfPoints = entryPoints.Count-1;
    }

    public void MoveToNextPoint()
    {
        UIEffects.instance.FadeScreen(0.2f);
        //doing this so player doesn't teleport before the fade
        StartCoroutine(delayedMoveToNextPosition(0.2f));
    }

    IEnumerator delayedMoveToNextPosition(float _waitTime)
    {
        if (currentPoint < numberOfPoints) currentPoint += 1;
        else currentPoint = 0;
        yield return new WaitForSeconds(_waitTime);
        player.transform.position = entryPoints[currentPoint].position + new Vector3 (0,1,0);
        yield return null;
    }


}
