using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


/// <summary>
/// This class contains the methods that can be called from button clicks.
/// It essentially implements the logic of the menu as in *this* game, but the
/// PopUpMenu class is a singleton and nearly standalone.
/// </summary>
public class MenuLogic : MonoBehaviour
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


    /// <summary>
    /// This method is activated from the first button on the Menu. This button has different states
    /// based on how its called. It can be called from the Main menu in scene 0, as well as
    /// from the pop-up menu.
    /// </summary>
    public void StartClicked()
    {
        if (_menuState == MenuState.Pause)
        {
            PopUpMenu.Instance.ClosePopupMenu();
            GlobalGameState.Instance.ResumeGame();
        }
        else if (_menuState == MenuState.Vanquished)
        {
            PopUpMenu.Instance.ClosePopupMenu();
            GlobalGameState.Instance.ResetAndStartGame();
        }
        else
        {
            GlobalGameState.Instance.StartGame();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && _menuState == MenuState.Pause && PopUpMenu.Instance.IsShowing)
        {
            PopUpMenu.Instance.ClosePopupMenu();
            GlobalGameState.Instance.ResumeGame();
        }
    }

    public void ResetClicked()
    {
        PopUpMenu.Instance.ClosePopupMenu();
        GlobalGameState.Instance.ResetAndStartGame();
    }


    public void QuitClicked()
    {
        GlobalGameState.EndGame();
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
