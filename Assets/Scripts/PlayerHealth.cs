using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerHealth : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        Assert.IsNotNull(_animator);
    }
    public void Damage()
    {
        _animator.SetTrigger("PlayerVanquished");
        GlobalGameState.PauseGame();
    }
}
