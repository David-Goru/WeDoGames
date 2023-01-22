using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// <summary>
// Grid class for the pathfinding system. Stores the nodes for A* to work.
// </summary>
public class Grid : MonoBehaviour
{
    public bool displayGridGizmos; //Testing

    [Range(2, 4)]
    [SerializeField] private int divisionsPerVertex = 1;

    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;

    [Header("References")]
    [SerializeField] private BuildObject buildObject = null;

    private Node[,] grid;
    private float nodeDiameter;
    private int gridSizeX, gridSizeY;

    private Vector3 worldBottomLeft;
    private Vector3 worldPos;
    private bool walkable;
    private Transform parentTransform;

    public static Node[] neighbours;


    private void Awake()
    {
        if(buildObject.VertexSize == 0)
        {
            buildObject.SetVertexSize();
        }
        nodeRadius = buildObject.VertexSize / Mathf.Pow(2, divisionsPerVertex);
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.FloorToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.FloorToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
        neighbours = new Node[8];
    }

    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

    private void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        worldBottomLeft = transform.position 
            - Vector3.right * Mathf.Floor(gridWorldSize.x / nodeDiameter) * nodeDiameter / 2 
            - Vector3.forward * Mathf.Floor(gridWorldSize.y / nodeDiameter) * nodeDiameter / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                worldPos = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);

                Collider[] colliders = Physics.OverlapSphere(worldPos, nodeRadius, unwalkableMask);
                if (colliders.Length > 0)
                {
                    parentTransform = colliders[0].transform;
                    walkable = false;
                }
                else
                {
                    parentTransform = null;
                    walkable = true;
                }
                grid[x, y] = new Node(walkable, worldPos, x, y, parentTransform);
            }
        }
    }

    public void GetNeighbours(Node node)
    {
        var currentNeighbour = 0;

        for (var x = -1; x <= 1; x++)
        {
            for (var y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue; //This is not a neighbour, it's the node we passed as an argument
                
                var checkX = node.gridX + x;
                var checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) //This is a neighbour of node
                {
                    neighbours[currentNeighbour] = grid[checkX, checkY];
                }
                else neighbours[currentNeighbour] = null;
                currentNeighbour++;
            }
        }
    }

    public Node NodeFromWorldPos(Vector3 worldPosition)
    {
        float percentX = worldPosition.x / gridWorldSize.x + 0.5f; //(a + b / 2) / b == a / b + 0.5
        float percentY = worldPosition.z / gridWorldSize.y + 0.5f;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.FloorToInt(Mathf.Min(gridSizeX * percentX, gridSizeX - 1));
        int y = Mathf.FloorToInt(Mathf.Min(gridSizeY * percentY, gridSizeY - 1));
        return grid[x, y];
    }

    public void SetWalkableNodes(bool isWalkable, Vector3 nodePos, float nodeRange, Transform parentTransform)
    {
        Node[] firstNodes = GetFirstNodesOnSpawn(nodePos);
        List<Node> nodeList = new List<Node>();

        foreach (Node node in firstNodes)
        {
            nodeList.Add(node);
            node.walkable = isWalkable;

            if (!node.walkable) node.parentTransform = parentTransform;
            else node.parentTransform = null;
        }

        nodeRange--;

        runBFS(nodeList, nodeRange, isWalkable, parentTransform);
    }

    
    private Node[] GetFirstNodesOnSpawn(Vector3 nodePos)
    {
        Node[] nodeArray = new Node[4];

        nodeArray[0] = NodeFromWorldPos(nodePos + Vector3.right * nodeRadius + Vector3.forward * nodeRadius);
        nodeArray[1] = NodeFromWorldPos(nodePos + Vector3.left * nodeRadius + Vector3.forward * nodeRadius);
        nodeArray[2] = NodeFromWorldPos(nodePos + Vector3.right * nodeRadius + Vector3.back * nodeRadius);
        nodeArray[3] = NodeFromWorldPos(nodePos + Vector3.left * nodeRadius + Vector3.back * nodeRadius);

        return nodeArray;
    }

    private void runBFS(List<Node> nodeList, float range, bool isWalkable, Transform parentTransform)
    {
        List<Node> visitedNodes = nodeList;
        List<Node> nodesToVisit;

        while (range > 0)
        {
            nodesToVisit = nodeList.ToList();
            nodeList.Clear();
            foreach (Node node in nodesToVisit)
            {
                GetNeighbours(node);
                foreach (Node neighbour in neighbours)
                {
                    if (neighbour == null) continue;
                    if (!visitedNodes.Contains(neighbour))
                    {
                        nodeList.Add(neighbour);
                        visitedNodes.Add(neighbour);
                        neighbour.walkable = isWalkable;

                        if (!neighbour.walkable) neighbour.parentTransform = parentTransform;
                        else neighbour.parentTransform = null;
                    }
                }
            }
            range--;
        }
    }

    //This is only a visual hint
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if (grid != null && displayGridGizmos)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                Gizmos.DrawCube(n.worldPos, Vector3.one * (nodeDiameter - 0.05f));
            }
        }
    }

}
