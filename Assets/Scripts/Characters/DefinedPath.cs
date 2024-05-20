using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefinedPath : MonoBehaviour
{
    [Header("Refs and Setup")]
    public bool isLoop;
    public List<Transform> paths = new List<Transform>();
    public float speed = 3;
    public float waitTime = 0.5f;
    public float turnSpeed = 90;

    [Header("Editor")]
    public List<Color> pathColors = new List<Color>();

    private bool stopFollowPath;

    private void Start()
    {
        
    }
    public void FollowPath(int pathIndex)
    {
        Transform pathHolder = paths[pathIndex];
        Vector3[] waypoints = new Vector3[pathHolder.childCount];
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = pathHolder.GetChild(i).position;
            waypoints[i] = new Vector3(waypoints[i].x, waypoints[i].y + 1, waypoints[i].z);
        }
        StartCoroutine(FollowPath(waypoints));
    }

    public void StopFollow()
    {
        stopFollowPath = true;
    }

IEnumerator FollowPath(Vector3[] waypoints)
    {
        transform.position = waypoints[0];

        int targetWaypointIndex = 1;
        Vector3 targetWaypoint = waypoints[targetWaypointIndex];
        transform.LookAt(targetWaypoint);

        while(targetWaypointIndex < waypoints.Length-1 || isLoop)
        {
            if (stopFollowPath) yield break;

            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, speed * Time.deltaTime);
            if (transform.position == targetWaypoint)
            {
                targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;
                targetWaypoint = waypoints[targetWaypointIndex];
                yield return new WaitForSeconds(waitTime);
                yield return StartCoroutine(TurnToFace(targetWaypoint));
            }
            yield return null;
        }
        //reset bool
        stopFollowPath = false;
        yield break;
    }

    IEnumerator TurnToFace(Vector3 lookTarget)
    {
        Vector3 dirToLookTarget = (lookTarget - transform.position).normalized;
        float targetAngle = 90 - Mathf.Atan2(dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;

        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05)
        {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
            yield return null;
        }
    }



    private void OnDrawGizmos()
    {
        foreach (Transform pathHolder in paths)
        {
           Gizmos.color = pathColors[paths.IndexOf(pathHolder)];

            Vector3 startPosition = pathHolder.GetChild(0).position;
            Vector3 previousPosition = startPosition;

            foreach (Transform waypoint in pathHolder)
            {
                Gizmos.DrawSphere(waypoint.position, 0.3f);
                Gizmos.DrawLine(previousPosition, waypoint.position);
                previousPosition = waypoint.position;
            }
            Gizmos.DrawLine(previousPosition, startPosition);
        } 
    }

}
