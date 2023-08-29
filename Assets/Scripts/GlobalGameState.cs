using System;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Contains information about whether the game is running or paused. This could be because
/// The pause menu is open, the game is over, ending animation is playing or some new condition.
/// 
/// This is a singleton object, which does not require a GameObject. It creates itself when
/// Instance is first referenced via the Instance property
/// </summary>
public class GlobalGameState : MonoBehaviour
{
    public bool IsPaused { get => _isPaused; }
    [SerializeField] private Boolean _isPaused = false;

    private bool _isMenuOpen = false;

    // TODO - get this working with multiple scenes
    private readonly List<Rigidbody2D> _rigidbodiesToPause = new();

    private float _oldTimeScale = 1f;

    private GameObject _canvasPrefab = null;
    private GameObject _canvasParent = null;
    private Canvas _canvasComponent = null;

    private GameObject _menuPrefab = null;
    private GameObject _menu = null;

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
        if (!_isMenuOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            _isMenuOpen = true;
            PauseGame();
            OpenPauseMenu();
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(_instance);
        else
            _instance = this;
    }


    public void PauseGame()
    {
        _isPaused = true;
        PauseRigidbodies();

        _oldTimeScale = Time.timeScale;
        Time.timeScale = 0;
    }


    public void UnpauseGame()
    {
        _isPaused = false;
        UnpauseRigidbodies();
        Time.timeScale = _oldTimeScale;
    }

    public void RegisterRigidbody(Rigidbody2D rigidBody)
    {
        _rigidbodiesToPause.Add(rigidBody);
    }

    private void PauseRigidbodies()
    {
        foreach (Rigidbody2D body in _rigidbodiesToPause)
        {
            body.simulated = false;
        }
    }
    private void UnpauseRigidbodies()
    {
        foreach (Rigidbody2D body in _rigidbodiesToPause)
        {
            body.simulated = true;
        }
    }


    private void OpenPauseMenu()
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

        Debug.Log("Set menu active");
        _menu.SetActive(true);
        Debug.Log("Set menu active finished");

    }

}
