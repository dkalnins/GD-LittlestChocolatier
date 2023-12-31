using UnityEngine;
using UnityEngine.Assertions;


/// <summary>
/// Covers off contoller and keyboard input and translates them into player movement.
/// 
/// Tracks the different move mode the player is in and updates the animator accordingly
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
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


    void Update()
    {
        if (GlobalGameState.Instance.IsPaused)
            return;

        // We want the player to press the jump button each time they want to jump, and not just be able to hold down
        // the button/key. We record it here so we can handle other stuff in UpdateFixed(), rather  than every frame in Update()
        if (!_jumpButtonPressed && Input.GetButtonDown("Jump"))
            _jumpButtonPressed = true;
    }

    void FixedUpdate()
    {
        if (GlobalGameState.Instance.IsPaused)
            return;

        HandleMovementControls();
    }

    private void HandleMovementControls()
    {
        // use GetAxisRaw so there is no "drift"
        float xControllerAxis = Input.GetAxisRaw("Horizontal");

        // Use the button press captured in Update, if any
        bool jumpPressed = _jumpButtonPressed;
        if (_jumpButtonPressed)
            _jumpButtonPressed = false;
;

        UpdateFacing(xControllerAxis);
        UpdateVelocityAndAnimationState(xControllerAxis, jumpPressed);

        PostAnimationState();        
    }

    private void UpdateVelocityAndAnimationState(float xControllerAxis, bool jumpPressed)
    {
        bool isGrounded = IsGrounded();

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
        if (_rigidBody.velocity.y < 0)
        {
            _movementState = MovementState.Falling;
        }
    }

    /// <summary>
    /// Change the player's facing from left to right or right to left, depending on its previous state.
    /// 
    /// Note that the attack system assumes that player flipping is done using this trick of multiplying
    /// the x-axis by -1, so that the attack target also flips, since its a child of the Player object
    /// </summary>
    private void FlipCharacter()
    {
        this.transform.localScale = new Vector3(this.transform.localScale.x * -1, this.transform.localScale.y, this.transform.localScale.z);
    }

    /// <summary>
    /// Updates the facing of the player based on the direction the controller is pointing.
    /// </summary>
    /// <param name="xController">x-axis value from the input manager</param>
    private void UpdateFacing(float xController)
    {
        if (xController > _xAxisZeroEquivalence && _playerFacing == PlayerFacing.Left)
        {
            _playerFacing = PlayerFacing.Right;
            FlipCharacter();
        }
        else if (xController < -_xAxisZeroEquivalence && _playerFacing == PlayerFacing.Right)
        {
            _playerFacing = PlayerFacing.Left;
            FlipCharacter();
        }
    }

    /// <summary>
    /// Push the current animation state to the Animator attached the player
    /// </summary>
    private void PostAnimationState()
    {
        _animator.SetInteger("AnimationState", (int)_movementState);
    }

}
