using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using UnityEngine;

// <summary>
// A* algorithm (modified)
// </summary>
public class Pathfinding : MonoBehaviour
{

    private List<Node> path;
    private Grid grid;
    private PathRequestManager requestManager;

    private Node startNode;
    private Node targetNode;
    private Node currentNode;
    private Node retraceCurrentNode;

    private Heap<Node> openSet;
    private HashSet<Node> closedSet;

    private bool pathSuccess;
    private int newMovementCostToNeighbour;

    //For helping me find the distance between two nodes
    private int distanceX;
    private int distanceY;

    //For testing diagnostics and optimization
    private Stopwatch sw;

    private void Awake()
    {
        grid = GetComponent<Grid>();
        requestManager = GetComponent<PathRequestManager>();
    }

    public void StartFindPath(Vector3 startPos, BuildingRange targetPos, float range)
    {
        StartCoroutine(FindPath(startPos, targetPos, range));
    }

    private IEnumerator FindPath(Vector3 startPos, BuildingRange targetPos, float range)
    {
        sw = new Stopwatch();
        sw.Start();

        Vector3[] waypoints = new Vector3[0];
        pathSuccess = false;

        startNode = grid.NodeFromWorldPos(startPos);
        targetNode = grid.NodeFromWorldPos(targetPos.transform.position);

        if(startNode.walkable) //For optimization
        {
            openSet = new Heap<Node>(grid.MaxSize);
            closedSet = new HashSet<Node>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (GetDistanceInNodes(currentNode, targetNode) < targetPos.Range + range) //Path has been found
                {
                    sw.Stop();
                    //print("Path found in " + sw.ElapsedMilliseconds + " ms");
                    pathSuccess = true;
                    break;
                }

                foreach (Node neighbour in grid.GetNeighbours(currentNode))
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour)) continue; //if neighbour is not traversable or is in closedSet, skip to next neighbour

                    newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);

                    //if new path to neighbour is shorter or neighbour is not in openSet
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour)) //add the neighbour to openSet if it's not already in it
                        {
                            openSet.Add(neighbour);
                        }
                    }
                }
            }
        }
        
        yield return null;
        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, currentNode);
        }
        requestManager.FinishedProcessingPath(waypoints, pathSuccess);
    }

    private Vector3[] RetracePath(Node startNode, Node endNode) //Retrace the given path
    {
        path = new List<Node>();
        retraceCurrentNode = endNode;

        while(retraceCurrentNode != startNode)
        {
            path.Add(retraceCurrentNode);
            retraceCurrentNode = retraceCurrentNode.parent;
        }
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);

        return waypoints;
    }

    private Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for(int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if(directionNew != directionOld)
            {
                waypoints.Add(path[i].worldPos);
            }
            directionOld = directionNew;
        }

        return waypoints.ToArray();
    }

    private int GetDistance(Node nodeA, Node nodeB)
    {
        distanceX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        distanceY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        //If x > y --> 14y + 10(x-y)
        //If y > x --> 14x + 10(y-x)
        if(distanceX > distanceY)
        {
            return 14 * distanceY + 10 * (distanceX - distanceY);
        }
        return 14 * distanceX + 10 * (distanceY - distanceX);
    }

    private int GetDistanceInNodes(Node nodeA, Node nodeB)
    {
        distanceX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        distanceY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        //If x > y --> y + (x-y)
        //If y > x --> x + (y-x)
        if (distanceX > distanceY)
        {
            return distanceY + (distanceX - distanceY);
        }
        return distanceX + (distanceY - distanceX);
    }

}
