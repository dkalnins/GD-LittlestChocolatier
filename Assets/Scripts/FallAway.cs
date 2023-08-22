using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class FallAway : MonoBehaviour
{

    [SerializeField] private float _secondsBeforeFall = 3f;
    //[SerializeField] private float _fallVelocity = 5f;
    
    [SerializeField] private bool _startedTimer = false;
    [SerializeField] private bool _fallTriggered = false;

    private float _fallTimer = 0;

    private Rigidbody2D _rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        Assert.IsNotNull(_rigidBody);
    }

    // Update is called once per frame
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
