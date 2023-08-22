using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script controls the way animation changes on the Fire Hazard. 
/// </summary>
[RequireComponent(typeof(Animator))]
public class FireHazard : MonoBehaviour
{
    [SerializeField] private bool _isAwake = false;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        if (_animator == null )
        {
            Debug.Log($"Could not find Animation in class {GetType().Name}  attached to {gameObject.name}");
        }
        PushAnimationStatus();
    }

    private void Update()
    {
        PushAnimationStatus();
    }

    public void ToggleAwakeStatus()
    {
        _isAwake = !_isAwake;
        PushAnimationStatus();
    }

    private void PushAnimationStatus()
    {
        _animator.SetBool("IsAwake", _isAwake);
    }
}