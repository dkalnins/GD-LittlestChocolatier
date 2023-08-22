using UnityEngine;

/// <summary>
/// Hazards and mobiles that damage the player. Currently health system is a single hit vanquish
/// </summary>
public class DamageOnTouchHazard : MonoBehaviour
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
