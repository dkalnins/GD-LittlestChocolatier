using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Menu : MonoBehaviour
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
        Debug.Log("StartClicked()");
        SceneManager.LoadScene(1);
    }

    public void ResetClicked()
    {
        Debug.Log("ResetClicked()");
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
        Debug.Log("Inside SetMenuType " + ((int)state).ToString());

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
