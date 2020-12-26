using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingAI : MonoBehaviour
{

    public BuildingRange target;

    [SerializeField] private float speed = 5f;
    private Vector3[] path;
    private int targetIndex;

    private Vector3 currentWaypoint;

    private void Start()
    {
        PathRequestManager.RequestPath(transform.position, target, OnPathFound);
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            StopCoroutine(FollowPath()); //This is for stopping the coroutine in case it's already running
            StartCoroutine(FollowPath());
        }
    }

    private IEnumerator FollowPath()
    {
        currentWaypoint = path[0];

        while (true)
        {
            if(transform.position == currentWaypoint)
            {
                targetIndex++;
                if(targetIndex >= path.Length) //Path finished
                {
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;
        }
    }

    //For visual hint
    public void OnDrawGizmos()
    {
        if(path != null)
        {
            for(int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawCube(path[i], Vector3.one/3);

                if(i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }

}
