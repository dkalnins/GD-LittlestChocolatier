using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MenuController : MonoBehaviour
{
    public enum MenuState { Main, Pause, Vanquished};
    MenuState _menuState = MenuState.Main;

    [SerializeField] private Text _startResumeButtonText;
    [SerializeField] private GameObject _restartButton;
    
    private void Awake()
    {
        Assert.IsNotNull(_startResumeButtonText);
        Assert.IsNotNull(_restartButton);

        SetMenuType(_menuState);
    }


    public void StartClicked()
    {
        if (_menuState == MenuState.Pause)
        {
            gameObject.SetActive(false);
            GlobalGameState.Instance.ResumeGame();
        }
        else if (_menuState == MenuState.Vanquished)
        {
            GlobalGameState.Instance.ResetGameAndStart();
        }
        else
        {
            GlobalGameState.Instance.StartGame();
        }
    }

    public void ResetClicked()
    {
        GlobalGameState.Instance.ResetGameAndStart();
    }


    public void QuitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void SetMenuType(MenuState state)
    {
        switch (state)
        {
            case MenuState.Main:
                _startResumeButtonText.text = "New Game";
                _restartButton.SetActive(false);
                break;

            case MenuState.Pause:
                _startResumeButtonText.text = "Resume";
                _restartButton.SetActive(true);
                break;

            case MenuState.Vanquished:
                _startResumeButtonText.text = "New Game";
                _restartButton.SetActive(false);
                break;
        }

        _menuState = state;
    }

}
