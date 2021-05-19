using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <summary>
// Node class for Grid matrix. Stores everything a node needs.
// </summary>
public class Node : IHeapItem<Node>
{

    public bool walkable; //Is the node is an obstacle?
    public Vector3 worldPos; //Position of the node
    public Transform parentTransform;

    public Node parent; //The parent of this node

    public int gCost; //distance travelled from the startPos
    public int hCost; //distance from the targetPos
    public int fCost //fCost = gCost + hCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    private int heapIndex;
    private int compare;

    //For helping me find the neighbours of a node
    public int gridX; //X position of the node in the grid matrix
    public int gridY; //Y position of the node in the grid matrix

    public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY, Transform _parentTransform)
    {
        walkable = _walkable;
        worldPos = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
        parentTransform = _parentTransform;
    }

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(Node nodeToCompare)
    {
        compare = fCost.CompareTo(nodeToCompare.fCost);
        if(compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }

        return -compare;
    }
}
