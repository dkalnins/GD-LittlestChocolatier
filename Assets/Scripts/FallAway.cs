using UnityEngine;
using UnityEngine.Assertions;


/// <summary>
/// Class used to implement experimental falling platform segments - no finished but does work!
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class FallAway : MonoBehaviour
{

    [SerializeField] private float _secondsBeforeFall = 3f;
   
    [SerializeField] private bool _startedTimer = false;
    [SerializeField] private bool _fallTriggered = false;

    private float _fallTimer = 0;

    private Rigidbody2D _rigidBody;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        Assert.IsNotNull(_rigidBody);
    }

    void Update()
    {
        if (_startedTimer)
            HandleTimer();
    }

    private void HandleTimer()
    {
        // Timers are paused if the game is paused
        if (GlobalGameState.IsPaused)
            return;

        _fallTimer += Time.deltaTime;
        if (_fallTimer >= _secondsBeforeFall)
        {
            _startedTimer = false;
            _fallTriggered = true;
            _rigidBody.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_fallTriggered && collision.CompareTag(("Player")))
            _startedTimer = true;
    }

}
