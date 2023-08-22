using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlyTouchHazard : MonoBehaviour
{

    private void Start()
    {
        if (!GetComponent<Collider2D>())
        {
            Debug.Log($"Could not find Collider2D in class {GetType().Name} attached to {gameObject.name}");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GlobalGameState.IsPaused)
            return;

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth pHealth = collision.GetComponent<PlayerHealth>();
            if (pHealth != null)
            {
                pHealth.Damage();
            }
            else
            {
                Debug.Log($"Could not find PlayerHealth in class {GetType().Name} attached to {collision.gameObject.name}");
            }
        }
    }
}
