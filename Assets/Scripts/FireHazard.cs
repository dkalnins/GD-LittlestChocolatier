using UnityEngine;

/// <summary>
/// This script controls the way animation changes on the Fire Hazard. The Fire
/// "wakes up" when the player approaches, and its collision box also increases.
/// </summary>
[RequireComponent(typeof(Animator))]
public class FireHazard : MonoBehaviour
{

    [SerializeField] private LayerMask _targetLayerMask;

    [SerializeField] private float _detectionRadius = 2.5f;
    [SerializeField] private float _growthMultiplierOnWake = 2f;

    [SerializeField] private bool _isAwake = false;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        if (_animator == null )
        {
            Debug.Log($"Could not find Animation in class {GetType().Name}  attached to {gameObject.name}");
        }

        if (_targetLayerMask == 0)
        {
            Debug.Log($"TargetLayerMask not specified in class {GetType().Name} attached to {gameObject.name}");
        }

        PushAnimationStatus();
    }


    private void Update()
    {
        if (GlobalGameState.Instance.IsPaused)
            return;

        if (!_isAwake)
            WakeupCheck();        
    }

    private void WakeupCheck()
    {
        if (Physics2D.OverlapCircle(transform.position, _detectionRadius, _targetLayerMask))
            WakeUp();
    }

    public void WakeUp()
    {
        _isAwake = true;
        transform.localScale = new Vector3(transform.localScale.x * _growthMultiplierOnWake, transform.localScale.x * _growthMultiplierOnWake, 1f);
        PushAnimationStatus();
    }

    private void PushAnimationStatus()
    {
        _animator.SetBool("IsAwake", _isAwake);
    }

    private void OnDrawGizmos()
    {
        /// Draw the circle where the attack occurs for calibration purposes
        Gizmos.color = Color.magenta;

        Gizmos.DrawWireSphere(transform.position, _detectionRadius);

    }
}
