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

    private GridManager _gridManager;
    private Dictionary<Vector2, Node> grid = new Dictionary<Vector2, Node>();

    [SerializeField] private Vector2 StartCoordinates;
    [SerializeField] private Vector2 DestinationCoordinates;

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
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        startNode = _gridManager.Grid[StartCoordinates];
        destinationNode = _gridManager.Grid[DestinationCoordinates];

        BreadthFirstSearch();
        BuildPath();
    }


    private void BreadthFirstSearch()
    {
        nodes_queue.Enqueue(startNode);
        NodesReached.Add(startNode.GridCoordinates, startNode);

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
}