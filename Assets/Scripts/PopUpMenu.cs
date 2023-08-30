using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// This singleton loads and assembles the PopUpMenu from Prefabs and manages
/// its state and activation/deactivation.
/// 
/// There is no logic embedded in her about what the buttons do or mean.
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

    private MenuController _menuController = null;

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

    public void OpenPopupMenu(MenuController.MenuState menuType)
    {
        _isShowing = true;
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
            _menuController = _pauseMenu.GetComponent<MenuController>();
            Assert.IsNotNull(_menuController);
        }
    }
}
