using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

/// <summary>
/// Contains information about whether the game is running or paused. This could be because
/// The pause menu is open, the game is over, ending animation is playing or some new condition.
/// 
/// This is a singleton object, which does not require a GameObject. It creates itself when
/// Instance is first referenced via the Instance property - i.e. lazy instantiation
/// </summary>
public class GlobalGameState : MonoBehaviour
{
    public bool IsPaused { get => (_isPlayerVanquished || PopUpMenu.Instance.IsShowing); }

    private bool _isPlayerVanquished = false;
    private int _currentLevel = 1;
    //private int _lastLevel = 4;
    private int _lastLevel = 1; // updated in Awake() below

    
    // Code to instantiate Singleton.
    private static GlobalGameState _instance;
    public static GlobalGameState Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = new GameObject("GlobalGameState").AddComponent<GlobalGameState>();
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(_instance);
        else
            _instance = this;

        _instance._lastLevel = SceneManager.sceneCountInBuildSettings - 1;
    }

    private void Update()
    {
        if (PopUpMenu.Instance.IsShowing)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isPlayerVanquished)
                OpenPopupMenu(MenuLogic.MenuState.Vanquished);
            else
                OpenPopupMenu(MenuLogic.MenuState.Pause);
        }
    }

    public void StartGame()
    {
        _currentLevel = 1;
        Cursor.visible = false;
        SceneManager.LoadScene(1);
    }
    public void ResumeGame()
    {
        Cursor.visible = false;
        StartTime();
    }

    public void ResetAndStartGame()
    {
        Cursor.visible = false;
        _isPlayerVanquished = false;
        StartTime();
        StartGame();
    }

    public void NextLevel()
    {
        Cursor.visible = false;
        _currentLevel++;
        if (_currentLevel > _lastLevel)
        {
            EndGame();
        }
        else
        {
            Cursor.visible = false;
            SceneManager.LoadScene(_currentLevel);
        }
    }

    public void PlayerVanquished()
    {
        _isPlayerVanquished = true;
        StopTime();
        PopUpMenu.Instance.OpenAfterDelay(2.5f);
    }

    private void StopTime()
    {
        Time.timeScale = 0;
    }

    private void StartTime()
    {
        Time.timeScale = 1f;
    }

    private void OpenPopupMenu(MenuLogic.MenuState menuType)
    {
        Cursor.visible = true;
        StopTime();
        PopUpMenu.Instance.OpenPopupMenu(menuType);
    }

    internal static void EndGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
