using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Animator))]
public class PlayerAttack : MonoBehaviour
{
    [SerializeField] Transform _attackLocation;

    [SerializeField] private float _attackRange = .2f;
    [SerializeField] private float _timeBeforeSwing = .2f;
    [SerializeField] private float _attackCooldown = .2f;

    private bool _isAttacking = false;
    private Animator _animator;

    private enum GizmoDebug { none, selected, always }
    [SerializeField, Header("Show Attack Area")]
    GizmoDebug _gizmoDebug = GizmoDebug.none;

    // Start is called before the first frame update
    void Start()
    {
        if (_attackLocation == null)
        {
            Debug.Log($"Could not find AttackLocation in class {GetType().Name} attached to {gameObject.name}");
        }

        _animator = GetComponent<Animator>();
        Assert.IsNotNull(_animator);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Attack") && !_isAttacking)
        {
            Attack();
        }

    }

    private void Attack()
    {
        Debug.Log("Start Attack");

        // Start attack animation
        _animator.SetTrigger("StartAttack");

        // see if we hit any enemies


        // apply damage
    }

    private void OnDrawGizmosSelected()
    {
        if (_gizmoDebug != GizmoDebug.selected)
            return;

        /// Draw the circle where the attack occurs for calibration purposes
        if (_attackLocation != null)
        {
            Gizmos.color = Color.yellow;

            Gizmos.DrawWireSphere(_attackLocation.position, _attackRange);
        }

    }
    private void OnDrawGizmos()
    {
        if (_gizmoDebug != GizmoDebug.always)
            return;

        /// Draw the circle where the attack occurs for calibration purposes
        if (_attackLocation != null)
        {
            Gizmos.color = Color.yellow;

            Gizmos.DrawWireSphere(_attackLocation.position, _attackRange);
        }

    }
}
