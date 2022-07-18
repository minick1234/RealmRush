using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Dictionary<Vector2, Node> GridOfNodes = new Dictionary<Vector2, Node>();

    [SerializeField] private GameObject GridContainer;
    [SerializeField] private GameObject[] tileObject;
    
    private void Awake()
    {
        GetAllTilesFromScene();
    }

    private void GetAllTilesFromScene()
    {
        tileObject = GameObject.FindGameObjectsWithTag("Tile");

        foreach (var tileItem in tileObject)
        {
            Vector2 coordinates = new Vector2(tileItem.gameObject.transform.position.x,
                tileItem.gameObject.transform.position.z);
            try
            {
                GridOfNodes.Add(coordinates, new Node(coordinates, true));
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