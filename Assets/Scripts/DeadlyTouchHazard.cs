using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlyTouchHazard : MonoBehaviour
{

    private void Start()
    {
        if (!GetComponent<Collider2D>())
        {
            //Debug.Log("Could not find Collider2D in class " +  GetType().Name + " attached to " + gameObject.name);
            Debug.Log($"Could not find Collider2D in class {GetType().Name}  attached to {gameObject.name}");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }
}
