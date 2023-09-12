using UnityEngine;

/// <summary>
/// Move the attached object between a set of points. Points are added in the unity editor and are
/// visited in a rotating sequence.
/// 
/// If there are more than 2 targets and a non-rotating order is needed, then points can be added
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

    [SerializeField]
    float _pauseDuration = 0;

    WaypointFlipper _waypointFlipper;
    Animator _animator;

    private void Start()
    {
        _waypointFlipper = GetComponent<WaypointFlipper>();
        _animator = GetComponent<Animator>();

        Debug.Assert(_waypoints.Length > 0);
    }

    void Update()
    {
        if (_pauseDuration > 0)
        {
            _pauseDuration -= Time.deltaTime;
            if (_pauseDuration <= 0 )
            {
                if (_animator)
                    _animator.speed = 1;
            }
        }

        if (GlobalGameState.Instance.IsPaused || _pauseDuration > 0)
            return;

        Move();
    }

    public void PauseFor(float pauseDuration)
    {
        // don't allow Enemies to be stun locked
        if (_pauseDuration <= 0)
            _pauseDuration = pauseDuration;
    }

    public void CancelPause()
    {
        _pauseDuration = 0;
        _animator.speed = 1;
    }

    private void Move()
    {
        if (_waypoints.Length < 1)
            return;

        float distanceToTarget = Vector2.Distance(transform.position, _waypoints[_targetWaypoint].position);

        if (distanceToTarget < _arrivalProximity)
        {
            _targetWaypoint++;
            _targetWaypoint %= _waypoints.Length;
        }

        if (_waypointFlipper != null)
            _waypointFlipper.InformXDirection(_waypoints[_targetWaypoint].position.x - transform.position.x);

        transform.position = Vector2.MoveTowards(transform.position, _waypoints[_targetWaypoint].position, _speed * Time.deltaTime);
    }
}
