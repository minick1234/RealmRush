using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] private List<Waypoint> _waypoints = new List<Waypoint>();

    [SerializeField] private float SecondsBeforeNextWaypoint = 2f;
    [SerializeField] private bool CurrentlyTraversing = false;
    [SerializeField] private Enemy _enemy;
    [SerializeField] private bool ReloopPath = false;
    [SerializeField] private bool DoneOnce = false;
    [SerializeField] private bool doneSpawning = false;

    [Range(0.25f, 5)] [SerializeField] private float TravelLength = 2f;

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


    private void Awake()
    {
        _enemy = this.gameObject.GetComponent<Enemy>();
    }

    //When this object is enabled or disabled and renabled this method will be called.
    void OnEnable()
    {
        DoneOnce = false;
        CurrentlyTraversing = false;
        FindPath();
        ReturnToStart();
    }

    private void Update()
    {
        if (_waypoints.Count > 0 && !CurrentlyTraversing && (!DoneOnce || ReloopPath))
        {
            //StartCoroutine(MoveToWayPoint());
            StartCoroutine(MoveToWayPointNoRemove());
            CurrentlyTraversing = true;
            DoneOnce = true;
        }
    }

    private IEnumerator MoveToWayPointNoRemove()
    {
        do
        {
            foreach (var waypoint in _waypoints)
            {
                //Before we make our move wait the amount of time between the reqauired waypoint
                yield return new WaitForSeconds(SecondsBeforeNextWaypoint);

                //Get the start and end point of this current waypoint connection
                Vector3 startPosition = this.gameObject.transform.position;
                Vector3 EndPosition = waypoint.gameObject.transform.position;

                //The travel percentage stores this current connections travel percantage that  has been completed.
                float TravelPercent = 0f;

                //Make the enemy constantly face the end position.
                //I can eventually make this slerp so it looks pretty and check when and if it even needs to be done.
                transform.LookAt(EndPosition);

                //Go over the whole connection of the path and keep traversing while we havent travesered the whole length.
                while (TravelPercent < TravelLength)
                {
                    //Increase the time.deltatime of the travel percentage.
                    TravelPercent += Time.deltaTime;
                    //Change this current gameobjects position based on the position that is returned 
                    //based on the original start and end point and the current position that we are at
                    //The reason we divide the travelPercent by the travel length is so that we can get a value that is relative to the total length so the speed is accurate.
                    //This could also just be done by multiplying the time.deltatime by a speed value and that will help to offset it as well.
                    this.gameObject.transform.position = Vector3.Lerp(startPosition,
                        EndPosition,
                        TravelPercent / TravelLength);
                    //before we exit this function we want to wait for the end of frame.
                    yield return new WaitForEndOfFrame();
                }
            }
        } while (ReloopPath);

        gameObject.SetActive(false);
        ObjectPool.DecreaseCurrentlyActiveEnemies();
        _enemy.StealPlayersMoney();
        CurrentlyTraversing = false;
    }


    public void ReturnToStart()
    {
        transform.position = _waypoints[0].transform.position;
        _waypoints.Remove(_waypoints[0]);
    }

    //this goes over every single item inside of the path gameobject and assigns it to our ai's path.
    private void FindPath()
    {
        //if we find a path we will clear the old path first.
        _waypoints.Clear();

        GameObject WaypointContainer = GameObject.FindGameObjectWithTag("Path");

        foreach (Transform waypoint in WaypointContainer.transform)
        {
            _waypoints.Add(waypoint.gameObject.GetComponent<Waypoint>());
        }
    }

    //This is not used for now as i dont want to remove the waypoints but it would be a viable options if i wanted to remove the waypoints from the list after visiting it.
    // private IEnumerator MoveToWayPoint()
    // {
    //     do
    //     {
    //         yield return new WaitForSeconds(SecondsBeforeNextWaypoint);
    //         Debug.Log("Traversing to new point");
    //         this.gameObject.transform.position = _waypoints[0].transform.position;
    //         _waypoints.RemoveAt(0);
    //     } while (_waypoints.Count > 0);
    //
    //     Debug.Log("All the waypoints have been traversed.");
    //     CurrentlyTraversing = false;
    // }
}