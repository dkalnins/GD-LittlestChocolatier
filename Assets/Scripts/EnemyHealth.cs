using UnityEngine;
using UnityEngine.Assertions;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int _maxHealth = 100;
    int _currentHealth;

    Animator _animator;

    private void Start()
    {
        _currentHealth = _maxHealth;

        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void ApplyDamage(int damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            HandleVanquished();
        }
        else
        {
            Debug.Log($"{GetType().Name} now has health {_currentHealth}");
            // TODO Apply Injured animation
        }
        
    }

    private void HandleVanquished()
    {
        // TODO Animation transition to vanquished?
        if (_animator)
        {
            _animator.SetBool("EnemyVanquished", true);
        }
        else
        {
            // If we don't have an animator
            transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
        }

        this.enabled = false;
        GetComponent<Collider2D>().enabled = false;

        WaypointMover waypointMover = GetComponent<WaypointMover>();
        if (waypointMover != null)
            waypointMover.enabled = false;

    }
}
