using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Player Physics")]
    [SerializeField] private float _walkSpeed = 7f;
    [SerializeField] private float _jumpVelocity = 12f;

    [Header("Controller Sensitivity")]
    [SerializeField] private float _xAxisZeroEquivalence = 0.001f;

    private Rigidbody2D _rigidBody;
    private Animator _animator;
    private BoxCollider2D _collider;
    private SpriteRenderer _spriteRenderer;

    [SerializeField]private LayerMask _jumpableLayer;


    private enum MovementState { Idle, Walking, Jumping, Falling };
    [SerializeField] private MovementState _movementState;

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

        _spriteRenderer = GetComponent<SpriteRenderer>();
        Assert.IsNotNull(_spriteRenderer);
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
    void FixedUpdate()
    {
        // in this game we can't chage direction in mid air
        HandleMovementControls();

    }

    private void HandleMovementControls()
    {
        // use GetAxisRaw so there is no "drift"
        float xControllerAxis = Input.GetAxisRaw("Horizontal");
        bool isGrounded = IsGrounded();
        bool jumpPressed = Input.GetButton("Jump");

        UpdateFacing(xControllerAxis);
        UpdateVelocityAndAnimationState(isGrounded, xControllerAxis, jumpPressed);

        PostAnimationState();        
    }

    private void UpdateVelocityAndAnimationState(bool isGrounded, float xControllerAxis, bool jumpPressed)
    {
        // Adjust horizontal veloctiy. Note that direction can change (currently) while in the air
        _rigidBody.velocity = new Vector2(xControllerAxis * _walkSpeed, _rigidBody.velocity.y);

        // We can only jump if on the ground
        if (jumpPressed && isGrounded)
        {
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, _jumpVelocity);
            _movementState = MovementState.Jumping;
            return;
        }

        // if we are on the ground and not moving, then we must be idle
        if (isGrounded && -_xAxisZeroEquivalence <= xControllerAxis && xControllerAxis <= _xAxisZeroEquivalence)
        {
            _movementState = MovementState.Idle;
            return;
        }

        // since we are on the ground we are walking
        if (isGrounded)
        {
            _movementState = MovementState.Walking;
            return;
        }

        // If we aren't grounded and our velocity is negative, then we are falling
        // TODO: probably want some kind of drift control here, in case the controller doesn't zero properly
        if (_rigidBody.velocity.y < 0)
        {
            _movementState = MovementState.Falling;
        }
    }

    private void UpdateFacing(float xController)
    {
        if (xController > _xAxisZeroEquivalence)
        {
            _spriteRenderer.flipX = true;
        }
        else if (xController < -_xAxisZeroEquivalence)
        {
            _spriteRenderer.flipX = false;
        }
    }

    private void PostAnimationState()
    {
        _animator.SetInteger("AnimationState", (int)_movementState);
    }
}
