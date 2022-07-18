using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    //HUGE NOTE ---- IN ORDER TO USE THE ONMOUSEXXX FUNCTIONS THE COLLIDER MUST BE ON THE SAME GAMEOBJECT OR YOU WILL RUN INTO MANY ISSUES.

    [SerializeField] private bool _IsPlaceableDefence = false;

    //this is another way to return is placeable without a method. - so now i can either use this or the other method below but this way is a little cleaner but both work regardless.
    public bool IsPlaceableDefence
    {
        get { return _IsPlaceableDefence; }
    }

    //this is super stupid and inefficient and should be in its own class or game manager where it will be able to be changed later.
    //but for right now whatever.
    [SerializeField] private Tower TowerObject;

    //The way this works is that when the mouse hovers over anything that has this function plus the collider on the same object
    //it makes a event call that triggers this and will run and execute each of the lines.
    private void OnMouseOver()
    {
        //     if (Input.GetMouseButtonDown(0))
        //     {
        //         Debug.Log("This is equivalent to the OnMouseDown, but it is just a little more intrusive and annoying.");
        //     }

        //this method will register every single frame when our mouse is over the collision tile.
        //Debug.Log(transform.name);
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

    //this is one way to make this accessible without the bool being public.
    // public bool GetIsPlaceableOnTile()
    // {
    //     return IsPlaceableDefence;
    // }
}