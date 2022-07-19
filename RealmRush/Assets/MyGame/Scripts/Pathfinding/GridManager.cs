using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Dictionary<Vector2, Node> GridOfNodes = new Dictionary<Vector2, Node>();

    [SerializeField] private int worldGridSize;

    [SerializeField]
    public int UnityGridSize
    {
        get { return worldGridSize; }
    }


    [SerializeField]
    public Dictionary<Vector2, Node> Grid
    {
        get { return GridOfNodes; }
    }


    [SerializeField] private GameObject[] tileObject;

    private void Awake()
    {
        GetAllTilesFromScene();
    }

    public Node GetNode(Vector2 coordinates)
    {
        if (GridOfNodes.ContainsKey(coordinates))
        {
            return GridOfNodes[coordinates];
        }

        return null;
    }

    public void BlockNode(Vector2 coordinatesOfNodeToBlock)
    {
        if (Grid.ContainsKey(coordinatesOfNodeToBlock))
        {
            Grid[coordinatesOfNodeToBlock].isWalkable = false;
        }
    }

    public Vector2 GetCoordinatesFromPosition(Vector3 position)
    {
        Vector2 coordinates = new Vector2();
        coordinates.x = Mathf.RoundToInt(position.x /
                                         UnityGridSize);
        coordinates.y = Mathf.RoundToInt(position.z /
                                         UnityGridSize);
        return coordinates;
    }

    public Vector3 GetPositionFromCoordinates(Vector2 coordinates)
    {
        Vector3 position = new Vector3();
        position.x = coordinates.x * UnityGridSize;
        position.y = coordinates.y * UnityGridSize;

        return position;
    }

    private void GetAllTilesFromScene()
    {
        tileObject = GameObject.FindGameObjectsWithTag("Tile");

        foreach (var tileItem in tileObject)
        {
            Vector2 coordinates = new Vector2(tileItem.gameObject.transform.position.x,
                tileItem.gameObject.transform.position.z);

            Debug.Log(tileItem.gameObject.transform.position.x + " is the x value and the z value is : " + tileItem.gameObject.transform.position.z + " and my gameobject name is: " + tileItem.name);
            
            try
            {
                Node node = new Node(coordinates, true);
                GridOfNodes.Add(coordinates, node);
            }
            catch (ArgumentException e)
            {
                Debug.Log("I have caught a second tile in the same location. i will remove it.");
                Debug.Log("The secondary tiles coordinates are: " + coordinates);
                Destroy(tileItem.gameObject);
            }
        }
    }
}