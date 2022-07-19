using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
    public Vector2 GridCoordinates;
    public bool isWalkable;
    public bool isExplored;
    public bool isPath;
    public bool isPlaceable = true;
    public Node connectedTo;

    public Node()
    {
    }

    public Node(Vector2 gridCoordinates, bool iswalkable)
    {
        GridCoordinates = gridCoordinates;
        isWalkable = iswalkable;
    }

    public Node(Vector2 gridCoordinates, bool iswalkable, bool isexplored, bool ispath, Node nodeconnectedto)
    {
        GridCoordinates = gridCoordinates;
        isWalkable = iswalkable;
        isExplored = isexplored;
        isPath = ispath;
        connectedTo = nodeconnectedto;
    }

    public Node(Vector2 gridCoordinates, bool iswalkable, bool isexplored, bool ispath)
    {
        GridCoordinates = gridCoordinates;
        isWalkable = iswalkable;
        isExplored = isexplored;
        isPath = ispath;
    }
}