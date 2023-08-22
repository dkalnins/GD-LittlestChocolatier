using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Simple health manager stub, which assumes we can only take one hit before being vanquished
/// </summary>
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
