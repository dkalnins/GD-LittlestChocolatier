using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int _maxHealth = 100;
    int _currentHealth;

    private void Start()
    {
        _currentHealth = _maxHealth;
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
        Debug.Log("Need an animation transition here");

        this.enabled = false;
        GetComponent<Collider2D>().enabled = false;
        WaypointMover waypointMover = GetComponent<WaypointMover>();
        if (waypointMover != null)
            waypointMover.enabled = false;
        
        // temporary substitute for animation
        transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);

    }
}
