using UnityEngine;

/// <summary>
/// Move the attached object between a set of points. Points are added in the unity editor and are
/// visited in a rotating sequence.
/// 
/// If there are more than 2 tarets and a non-rotating order is needed, then points can be added
/// into the loop multiple times. e.g. A, B, C, B this would then loop back and forth between three
/// locations.
/// 
/// If A, B, C are added (only) then the target will loop directly from C to A at the end of the cycle
/// 
/// Might wish to add delays at various points also.
/// </summary>
public class WaypointMover : MonoBehaviour
{
    // speed of the object between points. Transit is at the same speed between all points
    [SerializeField] private float _speed = 2.5f; 

    [SerializeField] private Transform[] _waypoints;

    int _targetWaypoint = 0;
    float _arrivalProximity = 0.1f;

    private void Start()
    {
        Debug.Assert(_waypoints.Length > 0);
    }

    void Update()
    {
        if (GlobalGameState.IsPaused)
            return;

        Move();
    }

    private void Move()
    {
        if (_waypoints.Length < 1)
            return;

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
