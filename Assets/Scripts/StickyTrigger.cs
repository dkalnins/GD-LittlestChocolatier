using UnityEngine;

/// <summary>
/// Added to the player so that if the player lands on it, they stick to it. USed
/// for moving platforms, but could be extended to allow mobs on it too
/// </summary>
public class StickyTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            collision.gameObject.transform.SetParent(transform);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            collision.gameObject.transform.SetParent(null);
    }
}
