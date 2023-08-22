using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Contains information about whether the game is running or paused. This could be because
/// The pause menu is open, the game is over, ending animation is playing or some new condition
/// </summary>
public static class GlobalGameState
{
    public static Boolean IsPaused { get => _isPaused; }
    private static Boolean _isPaused = false;

    private static List<Rigidbody2D> _rigidbodiesToPause = new List<Rigidbody2D>();

    private static float _oldTimeScale = 1f;

    public static void PauseGame()
    {
        _isPaused = true;
        PauseRigidbodies();

        _oldTimeScale = Time.timeScale;
        Time.timeScale = 0;
    }

    public static void UnpauseGame()
    {
        _isPaused = false;
        UnpauseRigidbodies();
        Time.timeScale = _oldTimeScale;
    }

    public static void RegisterRigidbody(Rigidbody2D rigidBody)
    {
        _rigidbodiesToPause.Add(rigidBody);
    }

    private static void PauseRigidbodies()
    {
        foreach(Rigidbody2D body in _rigidbodiesToPause)
        {
            body.simulated = false;
        }
    }
    private static void UnpauseRigidbodies()
    {
        foreach (Rigidbody2D body in _rigidbodiesToPause)
        {
            body.simulated = true;
        }
    }

}
