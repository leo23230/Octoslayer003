using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelNetwork : MonoBehaviour
{
    [Header("Details")]
    public string networkName;
    public int stealthScorePenalty;
    [Tooltip("Reason for stealth score, mostly for developers.")]
    public string explanation;
    private bool networkUsed = false;

    public List<Transform> gatheredEntryPoints = new List<Transform>();
    private LinkedList<Transform> entryPoints = new LinkedList<Transform>();

    private GameObject player;
    private AudioSource audioSource;

    private void Start()
    {
        player = GameObject.Find("Player");
        CollectGatheredNodesIntoLinkedList();
        Debug.Log(entryPoints.Count);
        audioSource = GetComponent<AudioSource>();
        //entryPoints = new List<Transform>(transform.GetComponentsInChildren<Transform>());
    }

    public void MoveToNextPoint(Transform _transform)
    {
        //Using Linked List to make sure each node in the network leads to the next
        LinkedListNode<Transform> currentNode = entryPoints.Find(_transform);
        Debug.Log("Current Position: " + currentNode.Value.position);
        Vector3 nextPosition;

        //this linked list is not circular, so we need to check if it's the last node and manually set it to first
        if (currentNode == entryPoints.Last) nextPosition = entryPoints.First.Value.position;
        else nextPosition = currentNode.Next.Value.position;

        Debug.Log("NextPosition: " + nextPosition);

        UIEffects.instance.FadeScreen(0.2f);

        //stop all coroutines on this behaviour
        StopAllCoroutines();
        //doing this so player doesn't teleport before the fade
        StartCoroutine(delayedMoveToNextPosition(nextPosition,0.2f));

        //play audio
        if (audioSource != null) audioSource.Play();

        //change player's stealth score if network is used for first time
        if (!networkUsed)
        {
            PlayerStealth.instance.SubtractStealth(stealthScorePenalty);
            networkUsed = true;
        }
    }

    IEnumerator delayedMoveToNextPosition(Vector3 _nextPosition, float _waitTime)
    {
        Debug.Log("started move");
        yield return new WaitForSeconds(_waitTime);
        player.GetComponent<Rigidbody>().position = _nextPosition + new Vector3 (0,1,0);
        yield return null;
    }

    public void CollectGatheredNodesIntoLinkedList()
    {
        foreach (Transform node in gatheredEntryPoints)
        {
            entryPoints.AddLast(node);
        }
    }


}
