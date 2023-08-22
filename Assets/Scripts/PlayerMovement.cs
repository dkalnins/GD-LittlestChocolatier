using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro.EditorUtilities;
using UnityEditor;
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

    private bool _jumpButtonPressed = false;


    private enum PlayerFacing { Left, Right };
    private PlayerFacing _playerFacing = PlayerFacing.Right;


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
        //TODO: If Grounded2() works, then this method and the _jumpableLayer might not be needed at all.
        return Physics2D.BoxCast(_collider.bounds.center, _collider.bounds.size, 0, Vector2.down, .1f, _jumpableLayer);
    }

    /// <summary>
    /// This gound check uses boxcast, to create a thin box "just" below the player's collider. This box is then cast
    /// "down" to see what it intersects.
    /// </summary>
    /// <returns></returns>
    bool IsGrounded2()
    {
        float boxDelta = 0.01f;
        float boxExtentY = 0.05f;

        Vector2 boxCenter = new(_collider.bounds.center.x, _collider.bounds.center.y -_collider.bounds.extents.y - boxDelta - boxExtentY);
        Vector2 boxSize = new(_collider.size.x, boxExtentY * 2);

        return Physics2D.BoxCast(boxCenter, boxSize, 0, Vector2.down, .1f);
    }

    void Update()
    {
        // We want the player to press the jump button each time they want to jump, and not just be able to hold down
        // the button/key. We record it here so we can handle other stuff in UpdateFixed(), rather  than every frame in Update()
        if (!_jumpButtonPressed && Input.GetButtonDown("Jump"))
            _jumpButtonPressed = true;
    }

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

        // Use the button press captured in Update, if any
        bool jumpPressed = _jumpButtonPressed;
        if (_jumpButtonPressed)
            _jumpButtonPressed = false;
;

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

    private void FlipCharacter()
    {
        this.transform.localScale = new Vector3(this.transform.localScale.x * -1, this.transform.localScale.y, this.transform.localScale.z);

    }

    private void UpdateFacing(float xController)
    {
        if (xController > _xAxisZeroEquivalence && _playerFacing == PlayerFacing.Left)
        {
            _playerFacing = PlayerFacing.Right;
            FlipCharacter();
            //_spriteRenderer.flipX = true;
        }
        else if (xController < -_xAxisZeroEquivalence && _playerFacing == PlayerFacing.Right)
        {
            _playerFacing = PlayerFacing.Left;
            FlipCharacter();
            //_spriteRenderer.flipX = false;
        }
    }

    private void PostAnimationState()
    {
        _animator.SetInteger("AnimationState", (int)_movementState);
    }
}
