using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    #region Script parameters

    [SerializeField]
    private List<Button> _menuButtons = new List<Button>();

    #endregion

    [SerializeField]
    private CanvasGroup _mainMenuPanel;
    [SerializeField]
    private CanvasGroup _creditsPanel;

    private GameObject _lastButtonSelected;

    public static MainMenuManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        SetMainMenu();
    }

    private void Start()
    {
        SetMenuButtons();
    }

    #region Buttons function

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void CreditsGame()
    {
        _mainMenuPanel.DOFade(0.0f, 1.0f).OnComplete(() =>
        {
            _mainMenuPanel.gameObject.SetActive(false);
            _creditsPanel.gameObject.SetActive(true);
            _creditsPanel.DOFade(1.0f, 1.0f);
        });
    }

    public void Back()
    {
        _creditsPanel.DOFade(0.0f, 1.0f).OnComplete(() =>
        {
            _creditsPanel.gameObject.SetActive(false);
            _mainMenuPanel.gameObject.SetActive(true);
            _mainMenuPanel.DOFade(1.0f, 1.0f);
        });
    }

    public void QuitGame()
    {
       #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif

        Application.Quit();
    }

    #endregion

    #region Menu functions
    private void SetMainMenu()
    {
        _mainMenuPanel.DOFade(1f, 0.1f);
        _creditsPanel.DOFade(0f, 0.1f);
    }

    private void SetMenuButtons()
    {
        foreach (Button button in _menuButtons)
            button.onClick.RemoveAllListeners();

        _menuButtons[0].onClick.AddListener(PlayGame); //Play button
        _menuButtons[1].onClick.AddListener(CreditsGame); //Credits button
        _menuButtons[2].onClick.AddListener(QuitGame); //Quit button
        _menuButtons[3].onClick.AddListener(Back); //Back button
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != null)
            _lastButtonSelected = EventSystem.current.currentSelectedGameObject;

        if (EventSystem.current.currentSelectedGameObject == null)
            EventSystem.current.SetSelectedGameObject(_lastButtonSelected);

        PressedSelectedButton();
    }

    private void PressedSelectedButton()
    {
        if (Input.GetKeyUp("joystick button 1"))
        {
            EventSystem.current.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
        }
    }
    #endregion
}
