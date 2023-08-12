using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Player Physics")]
    [SerializeField] private float _walkSpeed = 7f;
    [SerializeField] private float _jumpVelocity = 12f;

    private Rigidbody2D _rigidBody;
    private Animator _animator;
    private BoxCollider2D _collider;

    [SerializeField]private LayerMask _jumpableLayer;

    // Start is called before the first frame update
    void Start()
    {
        BindComponentVariables();
    }

    private void BindComponentVariables()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        Assert.IsNotNull(_rigidBody);

        _animator = GetComponent<Animator>();
        Assert.IsNotNull(_animator);

        _collider = GetComponent<BoxCollider2D>();
        Assert.IsNotNull(_collider);
    }


    /// <summary>
    /// Works out whether the player is "grounded", which can affect player behaviour and
    /// animations.
    /// 
    /// Grounded check is based off the the collider attached to the player.
    /// </summary>
    bool IsGrounded()
    {
        return Physics2D.BoxCast(_collider.bounds.center, _collider.bounds.size, 0, Vector2.down, .1f, _jumpableLayer);
    }

    // Update is called once per frame
    void Update()
    {
        float xDirection = Input.GetAxisRaw("Horizontal");

        _rigidBody.velocity = new Vector2(xDirection * _walkSpeed, _rigidBody.velocity.y);

        if (IsGrounded())
        {
            HandleMovementControls(xDirection);
        }



    }

    private void HandleMovementControls(float xDirection)
    {
        if (Input.GetButton("Jump"))
        {
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, _jumpVelocity);
        }

        if (xDirection >= 0.001f)
        {
            _animator.SetBool("isWalking", true);
        }
        else if (xDirection <= -0.001f)
        {
            _animator.SetBool("isWalking", true);
        }
        else
        {
            _animator.SetBool("isWalking", false);
        }
    }
}
