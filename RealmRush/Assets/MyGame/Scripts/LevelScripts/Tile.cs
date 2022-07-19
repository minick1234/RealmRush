using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    //HUGE NOTE ---- IN ORDER TO USE THE ONMOUSEXXX FUNCTIONS THE COLLIDER MUST BE ON THE SAME GAMEOBJECT OR YOU WILL RUN INTO MANY ISSUES.

    [SerializeField] private bool _IsPlaceableDefence;

    [SerializeField] private GridManager _gridManager;
    [SerializeField] private Vector2 coordinates = new Vector2();


    //this is super stupid and inefficient and should be in its own class or game manager where it will be able to be changed later.
    //but for right now whatever.
    [SerializeField] private Tower TowerObject;

    private void Awake()
    {
        _gridManager = FindObjectOfType<GridManager>();
    }

    private void Start()
    {
        if (_gridManager != null)
        {
            coordinates = _gridManager.GetCoordinatesFromPosition(gameObject.transform.position);

            if (!_IsPlaceableDefence)
            {
                _gridManager.BlockNode(coordinates);
            }
        }
    }


    //This is again working like on mouse over. Unity in the back is constantly checking for this event to fire and when it does, it checks for 
    //where this Onmousedown code is and then executes the instructions inside of it based on the collider that is attached to the same game object.
    private void OnMouseDown()
    {
        //Make sure that we are able to place down on this tile.
        if (_IsPlaceableDefence)
        {
            bool IsPlaced = TowerObject.CreateTower(TowerObject, transform.position);
            //Disable the ability to be able to place on this tile again since we spawned a new turret.
            this._IsPlaceableDefence = !IsPlaced;
        }
    }
}