using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Grid : MonoBehaviour
{
    public bool displayGridGizmos; //Testing

    [Range(2, 4)]
    [SerializeField] private int divisionsPerVertex = 1;

    //public Transform player; //Testing
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;

    [Header("References")]
    [SerializeField] private BuildObject buildObject;

    private Node[,] grid;
    private float nodeDiameter;
    private int gridSizeX, gridSizeY;

    private Vector3 worldBottomLeft;
    private Vector3 worldPos;
    private bool walkable;

    //For helping me find the neighbours of a node
    private List<Node> neighbours;
    private int checkX;
    private int checkY;


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
            for(int y = 0; y < gridSizeY; y++)
            {
                worldPos = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                walkable = !(Physics.CheckSphere(worldPos, nodeRadius, unwalkableMask));
                grid[x, y] = new Node(walkable, worldPos, x, y);
            }
        }
    }

    public List<Node> GetNeighbours(Node node)
    {
        neighbours = new List<Node>();

        for(int x = -1; x <= 1; x++)
        {
            for(int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue; //This is not a neighbour, it's the node we passed as an argument

                checkX = node.gridX + x;
                checkY = node.gridY + y;

                if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) //This is a neighbour of node
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
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

    public void SetWalkableNodes(bool isWalkable, Vector3 nodePos, float nodeRange)
    {
        Node[] firstNodes = GetFirstNodesOnSpawn(nodePos);
        List<Node> nodeList = new List<Node>();

        foreach(Node node in firstNodes)
        {
            nodeList.Add(node);
            node.walkable = isWalkable;
        }

        runBFS(nodeList, nodeRange, isWalkable);
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

    private void runBFS(List<Node> nodeList, float range, bool isWalkable)
    {
        List<Node> visitedNodes = nodeList;
        List<Node> nodesToVisit = new List<Node>();

        while (range > 0)
        {
            nodesToVisit = nodeList.ToList();
            nodeList.Clear();
            foreach(Node node in nodesToVisit)
            {
                foreach(Node neighbour in GetNeighbours(node))
                {
                    if (!visitedNodes.Contains(neighbour))
                    {
                        nodeList.Add(neighbour);
                        visitedNodes.Add(neighbour);
                        neighbour.walkable = isWalkable;
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
