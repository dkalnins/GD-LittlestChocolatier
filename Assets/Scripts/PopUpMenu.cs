using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// This singleton loads and assembles the PopUpMenu from Prefabs and manages
/// its state and activation/deactivation.
/// 
/// There is no logic embedded in her about what the buttons do or mean.
/// 
/// The canvas is loaded and defined separately in case the scene already has a
/// canvas, in which case the existing one is used first.
/// </summary>
public class PopUpMenu : MonoBehaviour
{
    private bool _isShowing = false;
    public bool IsShowing { get => _isShowing; }

    private GameObject _canvasPrefab = null;
    private GameObject _menuPrefab = null;

    private GameObject _canvasGameObject = null;
    private Canvas _canvasComponent = null;
    private GameObject _pauseMenu = null;

    private MenuLogic _menuController = null;

    private bool _popupAfterDelay = false;


    private static PopUpMenu _instance;
    public static PopUpMenu Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = new GameObject("PopUpMenu").AddComponent<PopUpMenu>();
                DontDestroyOnLoad(_instance.gameObject);

                _instance.InitializePopUp();
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
    }

    public void OpenPopupMenu(MenuLogic.MenuState menuType)
    {
        _isShowing = true;
        _popupAfterDelay = false; // in case the player hits escape while the counter is ticking
        _menuController.SetMenuType(menuType);
        _pauseMenu.SetActive(true);
    }

    public void ClosePopupMenu()
    {
        _isShowing = false;
        _pauseMenu.SetActive(false);
    }

    private void InitializePopUp()
    {
        LoadPrefabs();
        InstantiateMenu();

        DontDestroyOnLoad(_canvasGameObject);
    }

    private void LoadPrefabs()
    {
        if (!_canvasPrefab)
        {
            _canvasPrefab = Resources.Load<GameObject>("MenuCanvas");
            Assert.IsNotNull(_canvasPrefab);
        }
        if (!_menuPrefab)
        {
            _menuPrefab = Resources.Load<GameObject>("PauseMenu");
            Assert.IsNotNull(_menuPrefab);
        }
    }

    // TODO the idea behind defining the canvas seaparetly to the menu is to
    // use an existing canvas if there is one, but this case is not tested.
    private void InstantiateMenu()
    {
        if (!_canvasGameObject)
        {
            _canvasGameObject = Instantiate(_canvasPrefab);
            Assert.IsNotNull(_canvasGameObject);
        }
        if (!_canvasComponent)
        {
            _canvasComponent = _canvasGameObject.GetComponent<Canvas>();
            Assert.IsNotNull(_canvasComponent);
        }
        if (!_pauseMenu)
        {
            _pauseMenu = Instantiate(_menuPrefab, _canvasComponent.transform);
            Assert.IsNotNull(_pauseMenu);
            _pauseMenu.SetActive(false);
        }
        if (!_menuController)
        {
            _menuController = _pauseMenu.GetComponent<MenuLogic>();
            Assert.IsNotNull(_menuController);
        }
    }

    /// <summary>
    /// This method gets called when the game is paused because of player death. After the specified
    /// time 
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public void OpenAfterDelay(float delay)
    {
        _popupAfterDelay = true;
        StartCoroutine(CountDown(delay));
    }

    private IEnumerator CountDown(float countDown)
    {
        while (countDown > 0)
        {
            yield return new WaitForSeconds(1);
            countDown--;
        }

        if (_popupAfterDelay)
        {
            _popupAfterDelay = false;
            OpenPopupMenu(MenuLogic.MenuState.Pause);
        }
    }
}
