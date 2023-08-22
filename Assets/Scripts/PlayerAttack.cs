using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Handles controller input for attacks, delays before and after attacks, targetting
/// and then calls relevant damage functions on hit targets.
/// 
/// _attackLocation needs to a transform that is a child of the Player, which indicates where
/// the attack will be centred from. It raidiates out as a circle from there using the supplied
/// radius.
/// </summary>
[RequireComponent(typeof(Animator))]
public class PlayerAttack : MonoBehaviour
{
    [SerializeField] Transform _attackLocation;
    [SerializeField] LayerMask _targetLayerMask;

    [SerializeField] private float _attackRange = .2f;
    [SerializeField] private float _timeBeforeSwing = .2f;
    [SerializeField] private float _attackCooldown = .2f;

    private float _timer = 0f;
    private bool _beforeSwing = true;

    private bool _isAttacking = false;
    private Animator _animator;

    private enum GizmoDebug { none, selected, always }
    [SerializeField, Header("Show Attack Area")]
    GizmoDebug _gizmoDebug = GizmoDebug.none;

    void Start()
    {
        if (_attackLocation == null)
        {
            Debug.Log($"Could not find AttackLocation in class {GetType().Name} attached to {gameObject.name}");
        }

        if (_targetLayerMask == 0)
        {
            Debug.Log($"TargetLayerMask not specified in class {GetType().Name} attached to {gameObject.name}");
        }

        _animator = GetComponent<Animator>();
        Assert.IsNotNull(_animator);
    }

    private void Update()
    {
        if (GlobalGameState.IsPaused)
            return;

        if (_isAttacking)
            ContinueAttack();
        else if (Input.GetButtonDown("Attack"))
            StartAttack();

    }

    /// <summary>
    /// We are in the middle of an attack, either before the actual strike because there is a time delay
    /// after the button press, or after the actual swing during the cooldown.
    /// </summary>
    private void ContinueAttack()
    {
        _timer += Time.deltaTime;
        if (_beforeSwing)
        {
            if (_timer >= _timeBeforeSwing)
            {
                DoStrike();
                _timer = 0f;
                _beforeSwing = false;
            }
        }
        else // after swing
        {
            if (_timer >= _attackCooldown)
            {
                // cooldown is over, so the player may attack again
                _timer = 0f;
                _beforeSwing = true;
                _isAttacking = false;
            }
        }
    }

    private void DoStrike()
    {
        // Lets see if we hit anything (must be in layer _targetLayerMask)
        Collider2D[] hits = Physics2D.OverlapCircleAll(_attackLocation.position, _attackRange, _targetLayerMask);

        foreach (Collider2D hit in hits)
        {
            Debug.Log("a hit!");
        }
    }

    private void StartAttack()
    {
        // Start attack animation
        _animator.SetTrigger("StartAttack");
        _isAttacking = true;

        // setup timer that checks to see when swing occurs
        _timer = 0;
        _beforeSwing = true;
    }

    private void OnDrawGizmosSelected()
    {
        if (_gizmoDebug != GizmoDebug.selected)
            return;

        /// Draw the circle where the attack occurs for calibration purposes
        if (_attackLocation != null)
        {
            Gizmos.color = Color.magenta;

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
            Gizmos.color = Color.magenta;

            Gizmos.DrawWireSphere(_attackLocation.position, _attackRange);
        }

    }
}
