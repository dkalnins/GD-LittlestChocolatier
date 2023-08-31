using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTransition : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GlobalGameState.Instance.IsPaused)
            return;

        if (collision.gameObject.CompareTag("Player"))
        {
            GlobalGameState.Instance.NextLevel();
        }
    }
}
