using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Tracks enemy health, damage, and when to convert into a vanquished state
/// 
/// Also applies animations
/// </summary>
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int _maxHealth = 100;
    int _currentHealth;
    [SerializeField] float _stunDuration = .3f;

    Animator _animator;
    WaypointMover _waypointMover;

    private void Start()
    {
        _currentHealth = _maxHealth;

        _animator = GetComponent<Animator>();
        _waypointMover = GetComponent<WaypointMover>();
    }

    public void ApplyDamage(int damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            HandleVanquished();
        }
        else
        {
            //Debug.Log($"{GetType().Name} now has health {_currentHealth}");
            if (_waypointMover)
            {
                _waypointMover.PauseFor(_stunDuration);
                if (_animator)
                    _animator.speed = 0;
            }
            
        }        
    }

    private void HandleVanquished()
    {
        if (_waypointMover)
            _waypointMover.CancelPause();

        if (_animator)
        {
            _animator.SetBool("EnemyVanquished", true);
        }
        else
        {
            // Flip the vanquished target if we don't have an animator
            transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
        }

        this.enabled = false;
        GetComponent<Collider2D>().enabled = false;

        WaypointMover waypointMover = GetComponent<WaypointMover>();
        if (waypointMover != null)
            waypointMover.enabled = false;

    }
}
