using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains information about whether the game is running or paused. This could be because
/// The pause menu is open, the game is over, ending animation is playing or some new condition
/// </summary>
public static class GlobalGameState
{
    public static Boolean IsPaused { get => _isPaused; }
    private static Boolean _isPaused = false;

    private static List<Rigidbody2D> _rigidBodiesToPause = new List<Rigidbody2D>();

    public static void PauseGame()
    {
        _isPaused = true;
        PauseRigidBodies();
        Time.timeScale = 0;
    }

    public static void UnpauseGame()
    {
        _isPaused = false;
        UnpauseRigidBodies();
        Time.timeScale = 1;
    }

    public static void RegisterRigidBody(Rigidbody2D rigidBody)
    {
        _rigidBodiesToPause.Add(rigidBody);
    }

    private static void PauseRigidBodies()
    {
        foreach(Rigidbody2D body in _rigidBodiesToPause)
        {
            body.simulated = false;
        }
    }
    private static void UnpauseRigidBodies()
    {
        foreach (Rigidbody2D body in _rigidBodiesToPause)
        {
            body.simulated = true;
        }
    }

}
