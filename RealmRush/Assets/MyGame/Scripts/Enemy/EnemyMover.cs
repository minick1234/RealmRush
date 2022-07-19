using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Object = System.Object;

[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField] private List<Node> _waypoints = new List<Node>();
    [SerializeField] private Pathfinder _pathfinder;
    [SerializeField] private GridManager _gridManager;
    [SerializeField] private Enemy _enemy;
    [Range(0.25f, 5)] [SerializeField] private float TravelSpeed = 2f;
    [SerializeField] private float TimeBetweenNextWaypoint;

    private void Awake()
    {
        _pathfinder = FindObjectOfType<Pathfinder>();
        _gridManager = FindObjectOfType<GridManager>();

        FindPath(true);
        ReturnToStart();
        _enemy = this.gameObject.GetComponent<Enemy>();
    }

    //When this object is enabled or disabled and renabled this method will be called.
    void OnEnable()
    {
        ReturnToStart();
        FindPath(true);
    }

    private IEnumerator MoveToWayPointNoRemove()
    {
        for (int i = 1; i < _waypoints.Count; i++)
        {
            yield return new WaitForSeconds(TimeBetweenNextWaypoint);
            Vector3 startPosition = transform.position;
            Vector3 endPosition = _gridManager.GetPositionFromCoordinates(_waypoints[i].GridCoordinates);
            float travelPercent = 0f;

            transform.LookAt(endPosition);

            while (travelPercent < 1f)
            {
                travelPercent += Time.deltaTime * TravelSpeed;
                transform.position = Vector3.Lerp(startPosition, endPosition, travelPercent);
                yield return new WaitForEndOfFrame();
            }
        }

        FinishMyPath();
    }

    private void FinishMyPath()
    {
        gameObject.SetActive(false);
        ObjectPool.DecreaseCurrentlyActiveEnemies();
        _enemy.StealPlayersMoney();
    }

    public void ReturnToStart()
    {
        transform.position = _gridManager.GetPositionFromCoordinates(_pathfinder.startCoordinates);
    }

    private void FindPath(bool resetPath)
    {
        Vector2 coordinates = new Vector2();

        if (resetPath)
        {
            coordinates = _pathfinder.startCoordinates;
        }
        else
        {
            coordinates = _gridManager.GetCoordinatesFromPosition(gameObject.transform.position);
        }

        StopAllCoroutines();
        _waypoints.Clear();
        _waypoints =
            _pathfinder.GetNewPath(coordinates);
        StartCoroutine(MoveToWayPointNoRemove());
    }
}