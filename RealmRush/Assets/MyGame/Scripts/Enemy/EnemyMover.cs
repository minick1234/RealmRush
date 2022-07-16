using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] private List<Waypoint> _waypoints = new List<Waypoint>();

    [SerializeField] private float SecondsBeforeNextWaypoint = 2f;
    [SerializeField] private bool CurrentlyTraversing = false;

    [SerializeField] private bool ReloopPath = false;


    //The way he initially implemented moving through the waypoints and there names is by going and making a 
    //InvokeRepeating method which repeats a method a certain number of times you specify.
    //which is incredble, the way i had implemented going through each point and moving there is by 
    //making a coroutine and going through it and making sure we wait a specific delay before going through
    //to the next position and if we are going through and traversing we dont allow the update to go inside.
    //until we have fully traversed all the items.
    //after that it doesnt access the traversal method and start that coroutine until the waypoint list is greater then 0 again.
    //therefore emitting the ability to access that inefficiently.

    //inside of the movetowaypoint coroutine i use a do while loop, and just remove each element from the list after i use it which is ok
    //but i would also be completly valid in using a foreach loop to go over every element and just printing and then waiting for the seconds delay. The only difference is that
    //this is keen if i want to keep the wayspoints in the enemies list after i have traversed them but my initial thought was after i pass that point i pick a new spot to move to instead.


    //a problem we are faced with in the video is that when we add into the list it randomizes the order we want our path to be

    //a solution to this by code would be just to check which tile we are at and if it is greater then or less then the
    //previous one and check what the next one after that is to distinquish the predicted desired path, if not we can always change up the order through code.


    // Start is called before the first frame update
    void Start()
    {
    }

    private void Update()
    {
        if (_waypoints.Count > 0 && !CurrentlyTraversing)
        {
            //StartCoroutine(MoveToWayPoint());
            StartCoroutine(MoveToWayPointNoRemove());
            CurrentlyTraversing = true;
        }
    }

    private IEnumerator MoveToWayPointNoRemove()
    {
        while (ReloopPath)
        {
            foreach (var waypoint in _waypoints)
            {
                yield return new WaitForSeconds(SecondsBeforeNextWaypoint);
                Debug.Log("Traversing to new point");
                this.gameObject.transform.position = waypoint.gameObject.transform.position;
            }

            if (ReloopPath)
            {
                Debug.Log("The waypoints have been successfully traversed, relooping path.");
            }
            else
            {
                Debug.Log("The waypoints have been successfully traversed, back to original spot.");
            }
        }

        CurrentlyTraversing = false;
    }


    private IEnumerator MoveToWayPoint()
    {
        do
        {
            yield return new WaitForSeconds(SecondsBeforeNextWaypoint);
            Debug.Log("Traversing to new point");
            this.gameObject.transform.position = _waypoints[0].transform.position;
            _waypoints.RemoveAt(0);
        } while (_waypoints.Count > 0);

        Debug.Log("All the waypoints have been traversed.");
        CurrentlyTraversing = false;
    }
}