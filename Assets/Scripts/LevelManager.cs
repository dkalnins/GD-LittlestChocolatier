using System;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    static private LevelManager _instance;
    static public LevelManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = new GameObject().AddComponent<LevelManager>();
                DontDestroyOnLoad(_instance);
            }
            return _instance;
        }
    }

    // Todo - load list of scenes from folder, rather than hard coding (note that they sort alphabetically)
    private string[] _sceneList = { "Menu", "Level01", "Level02" };

    private int _currentLevel = 0;


    private void Awake()
    {
        // When we start up there will be one scene loaded - fine out which one it is

        // If its not the menu screen, we need to make sure the menu screen is loaded.


        string activeScene = SceneManager.GetActiveScene().name;
        for (int i = 0; i < _sceneList.Length; i++)
        {
            if (activeScene == _sceneList[i])
            {
                _currentLevel = i;
                break;
            }
        }


        Debug.Log($"LevelManager.Awake(): {SceneManager.GetActiveScene().name}");
    }

    // TODO Remove this method, since it will mask bugs. This is just being used at the start
    // of development to make sure that the singletons get loaded. This is primarily so I can understand
    // how I want the several singletons to interoperate. Perhaps they should be merged
    public void DebugLoad()
    {
        Debug.Log("LevelManager.DebugLoad()");
    }

    internal void PauseMenu()
    {
        throw new NotImplementedException();
    }
}
