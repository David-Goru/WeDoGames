using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

// <summary>
// Request class for pathfinding. Used by AI enemies.
// </summary>
public class PathRequestManager : MonoBehaviour
{
    private Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    private PathRequest currentPathRequest;

    private static PathRequestManager instance;
    private Pathfinding pathfinding;

    private bool isProcessingPath;

    private void Awake()
    {
        instance = this;
        pathfinding = GetComponent<Pathfinding>();
    }

    public static void RequestPath(Vector3 pathStart, PathData pathEnd, float range, Action<Vector3[], bool> callback)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, range, callback);
        instance.pathRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }

    private void TryProcessNext()
    {
        if(!isProcessingPath && pathRequestQueue.Count > 0)
        {
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd, currentPathRequest.range);
        }
    }

    public void FinishedProcessingPath(Vector3[] path, bool success)
    {
        currentPathRequest.callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }

    struct PathRequest
    {
        public Vector3 pathStart;
        public PathData pathEnd;
        public float range;
        public Action<Vector3[], bool> callback;

        public PathRequest(Vector3 _start, PathData _end, float _range, Action<Vector3[], bool> _callback)
        {
            pathStart = _start;
            pathEnd = _end;
            range = _range;
            callback = _callback;
        }
    }
}

public struct PathData
{
    public Vector3 targetPosition;
    public float buildingRange;

    public PathData(Vector3 _targetPosition, float _buildingRange = 0)
    {
        targetPosition = _targetPosition;
        buildingRange = _buildingRange;
    }
}