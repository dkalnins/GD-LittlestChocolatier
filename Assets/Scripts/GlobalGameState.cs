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
    public bool IsPaused { get => (_isMenuOpen || _isPlayerVanquished); }

    private bool _isPlayerVanquished = false;
    private bool _isMenuOpen = false;

    private GameObject _canvasPrefab = null;
    private GameObject _canvasParent = null;
    private Canvas _canvasComponent = null;

    private GameObject _menuPrefab = null;
    private GameObject _menu = null;

    private MenuController _menuController = null;

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

    private void Update()
    {
        if (_isMenuOpen)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _isMenuOpen = true;
            if (_isPlayerVanquished)
                OpenPopupMenu(MenuController.MenuState.Vanquished);
            else
                OpenPopupMenu(MenuController.MenuState.Pause);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void ResumeGame()
    {
        StartTime();
    }

    public void ResetAndStartGame()
    {
        _isPlayerVanquished = false;
        _isMenuOpen = false;
        StartTime();
        StartGame();
    }

    public void PlayerVanquished()
    {
        _isPlayerVanquished = true;
        StopTime();
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(_instance);
        else
            _instance = this;
    }


    private void StopTime()
    {
        Time.timeScale = 0;
    }

    private void StartTime()
    {
        Time.timeScale = 1f;
        _isMenuOpen = false;
    }


    // TODO this Pause menu feels pretty gross. Think about fixing it in some way.
    // TODO the names of the different components need to be fixed up at the very least
    private void OpenPopupMenu(MenuController.MenuState menuType)
    {
        if (!_canvasPrefab)
        {
            _canvasPrefab = Resources.Load<GameObject>("MenuCanvas");
            Assert.IsNotNull(_canvasPrefab);
        }
        if (!_canvasParent)
        {
            _canvasParent = Instantiate(_canvasPrefab);
            Assert.IsNotNull(_canvasParent);
        }
        if (!_canvasComponent)
        {
            _canvasComponent = _canvasParent.GetComponent<Canvas>();
            Assert.IsNotNull(_canvasComponent);
        }

        if (!_menuPrefab)
        {
            _menuPrefab = Resources.Load<GameObject>("PauseMenu");
            Assert.IsNotNull(_menuPrefab);
        }
        if (!_menu)
        {
            _menu = Instantiate(_menuPrefab, _canvasComponent.transform);
            Assert.IsNotNull(_menu);
        }
        if (!_menuController)
        {
            _menuController = _menu.GetComponent<MenuController>();
            Assert.IsNotNull(_menuController);
        }

        StopTime();
        _isMenuOpen = true;
        _menuController.SetMenuType(menuType);
        _menu.SetActive(true);

    }

}
