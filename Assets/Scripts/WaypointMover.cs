using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.UIElements;

public class WaypointMover : MonoBehaviour
{

    [SerializeField]
    private float _speed = 2.5f; 

    [SerializeField]
    private Transform[] _waypoints;

    int _targetWaypoint = 0;
    float _arrivalProximity = 0.1f;

    private void Start()
    {
        Debug.Assert(_waypoints.Length > 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalGameState.IsPaused)
            return;

        Move();
    }

    private void Move()
    {
        float distanceToTarget = Vector2.Distance(transform.position, _waypoints[_targetWaypoint].position);

        if (distanceToTarget < _arrivalProximity)
        {
            _targetWaypoint++;
            if (_targetWaypoint >= _waypoints.Length)
                _targetWaypoint = 0;
        }

        transform.position = Vector2.MoveTowards(transform.position, _waypoints[_targetWaypoint].position, _speed * Time.deltaTime);
    }
}
