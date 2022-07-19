using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    //these are all different possible directions i could go. - i want to make it random so different enemies check different directions.
    private Vector2[] directions1 = { Vector2.right, Vector2.left, Vector2.up, Vector2.down, };
    private Vector2[] directions2 = { Vector2.left, Vector2.right, Vector2.down, Vector2.up, };

    private Vector2[] directions3 = { Vector2.down, Vector2.left, Vector2.up, Vector2.right, };
    private Vector2[] directions4 = { Vector2.down, Vector2.up, Vector2.left, Vector2.right, };

    [SerializeField] private GridManager _gridManager;
    private Dictionary<Vector2, Node> grid = new Dictionary<Vector2, Node>();

    [SerializeField] private Vector2 StartCoordinates;

    public Vector2 startCoordinates
    {
        get { return StartCoordinates; }
    }

    [SerializeField] private Vector2 DestinationCoordinates;

    public Vector2 destinationCoordinates
    {
        get { return DestinationCoordinates; }
    }

    [SerializeField] private Node startNode;
    [SerializeField] private Node destinationNode;
    [SerializeField] private Node currentSearchNode;

    private Dictionary<Vector2, Node> NodesReached = new Dictionary<Vector2, Node>();
    private Queue<Node> nodes_queue = new Queue<Node>();

    private void Awake()
    {
        _gridManager = FindObjectOfType<GridManager>();
        if (_gridManager != null)
        {
            grid = _gridManager.Grid;
            startNode = grid[StartCoordinates];

            destinationNode = grid[DestinationCoordinates];
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GetNewPath();
    }

    public List<Node> GetNewPath()
    {
        _gridManager.ResetNodes();
        BreadthFirstSearch(startCoordinates);
        return BuildPath();
    }

    public List<Node> GetNewPath(Vector2 coordinates)
    {
        _gridManager.ResetNodes();
        BreadthFirstSearch(coordinates);
        return BuildPath();
    }

    private void BreadthFirstSearch(Vector2 coordinates)
    {
        startNode.isPlaceable = false;
        startNode.isWalkable = true;
        destinationNode.isPlaceable = false;
        destinationNode.isWalkable = true;

        nodes_queue.Clear();
        NodesReached.Clear();

        nodes_queue.Enqueue(_gridManager.Grid[coordinates]);
        NodesReached.Add(coordinates, _gridManager.Grid[coordinates]);

        while (nodes_queue.Count > 0)
        {
            currentSearchNode = nodes_queue.Dequeue();
            currentSearchNode.isExplored = true;
            ExploreNeighbours();
            if (currentSearchNode.GridCoordinates == destinationNode.GridCoordinates)
            {
                break;
            }
        }
    }

    private List<Node> BuildPath()
    {
        List<Node> path = new List<Node>();
        Node currentNode = destinationNode;

        path.Add(currentNode);
        currentNode.isPath = true;

        while (currentNode.connectedTo != null)
        {
            currentNode = currentNode.connectedTo;
            currentNode.isPath = true;
            path.Add(currentNode);
        }

        path.Reverse();
        return path;
    }


    private void ExploreNeighbours()
    {
        List<Node> Neighbours = new List<Node>();

        foreach (var direction in directions1)
        {
            Vector2 neighbourCoordinates = currentSearchNode.GridCoordinates + direction;

            if (grid.ContainsKey(neighbourCoordinates))
            {
                Neighbours.Add(grid[neighbourCoordinates]);
            }
        }

        foreach (var neighbour in Neighbours)
        {
            if (!NodesReached.ContainsKey(neighbour.GridCoordinates) && neighbour.isWalkable)
            {
                neighbour.connectedTo = currentSearchNode;
                NodesReached.Add(neighbour.GridCoordinates, neighbour);
                nodes_queue.Enqueue(neighbour);
            }
        }
    }

    //prevents the ability to block the path fully.
    public bool WillBlockPath(Vector2 coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            bool previousState = grid[coordinates].isWalkable;
            grid[coordinates].isWalkable = false;
            List<Node> newPath = GetNewPath();
            grid[coordinates].isWalkable = previousState;

            //this is not a valid path as the path will be blocked.
            if (newPath.Count <= 1)
            {
                GetNewPath();
                return true;
            }
        }

        return false;
    }

    public void NotifyRecievers()
    {
        BroadcastMessage("FindPath", false, SendMessageOptions.DontRequireReceiver);
    }
}