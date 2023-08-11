using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D _rb;
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float xDirection = Input.GetAxisRaw("Horizontal");

        _rb.velocity = new Vector2(xDirection * 7f, _rb.velocity.y);

        if (Input.GetButton("Jump"))
        {
            _rb.velocity = new Vector2(_rb.velocity.x, 7f);
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
