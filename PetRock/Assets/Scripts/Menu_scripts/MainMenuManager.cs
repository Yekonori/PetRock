using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    #region Script parameters

    [Header("Buttons")]
    [SerializeField]
    private List<Button> _menuButtons = new List<Button>();
    [SerializeField]
    private List<Button> _backMenuButtons = new List<Button>();

    #endregion

    [Header("Panels")]
    [SerializeField]
    private CanvasGroup _mainMenuPanel;
    [SerializeField]
    private CanvasGroup _optionsPanel;
    [SerializeField]
    private CanvasGroup _creditsPanel;

    [Header("Settings Panel")]
    [SerializeField]
    private List<GameObject> _settingsPanels = new List<GameObject>();
    [SerializeField]
    private TextMeshProUGUI _titleSettings;
    private const string _titleGraphics = "Graphics settings";
    private const string _titleAudio = "Audio settings";
    private int _indexSettingsPanel = 0;

    [Header("Animator")]
    [SerializeField]
    private Animator _creditsAnim;
    [SerializeField]
    private Animator _playerStartAnimator;

    [Header("Camera")]
    [SerializeField]
    private FreeFollowView _cam;
    [SerializeField]
    private Transform _rock;

    [Header("Tutorial")]
    [SerializeField]
    private GameObject _tutoCanvas;

    private GameObject _lastButtonSelected;
    private Image _backgroundMainMenu;
    private CanvasGroup _menuPanels;

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

        _backgroundMainMenu = GetComponent<Image>();
        _menuPanels = GetComponent<CanvasGroup>();

        _titleSettings.text = _titleGraphics;
    }

    private void Start()
    {
        SetMenuButtons();
        SetBackMenuButtons();

        GameManager.instance.TransitionCanvas(0).SetDelay(1);
    }

    #region Buttons function

    void PlayGame()
    {
        _menuPanels.DOFade(0, 1).OnComplete(() =>
        {
            _playerStartAnimator.SetBool("WakeUp", true);
            StartCoroutine(StartGame());
        });
    }

    IEnumerator StartGame()
    {
        yield return new WaitUntil(() => _playerStartAnimator.GetCurrentAnimatorStateInfo(0).IsName("StandingIdle"));

        GetComponent<StartDialogue_Script>().StartDialogue();

        _cam.ChangeTargetCam(_rock);

        yield return new WaitWhile(() => GetComponent<StartDialogue_Script>().CheckEndDialogue());

        GameManager.instance.inMainMenu = false;

        _tutoCanvas.SetActive(true);
        _tutoCanvas.GetComponent<Tutorial_Script>().IsFirstPlayerMovement();

        Destroy(gameObject);
    }

    void OptionsGame()
    {
        PanelSwitch(_mainMenuPanel, _optionsPanel);
    }

    void CreditsGame()
    {
        PanelSwitch(_mainMenuPanel, _creditsPanel);
        _backgroundMainMenu.DOFade(1, 1).OnComplete(() => _creditsAnim.SetBool("PlayCredits", true));
    }

    void BackCredits()
    {
        PanelSwitch(_creditsPanel, _mainMenuPanel);
        _backgroundMainMenu.DOFade(0.5f, 1);
    }

    void BackOptions()
    {
        PanelSwitch(_optionsPanel, _mainMenuPanel);
    }

    void PanelSwitch(CanvasGroup firstPanel, CanvasGroup secondPanel)
    {
        firstPanel.DOFade(0.0f, 1.0f).OnComplete(() =>
        {
            firstPanel.gameObject.SetActive(false);
            secondPanel.gameObject.SetActive(true);
            secondPanel.DOFade(1.0f, 1.0f);
        });
    }

    void QuitGame()
    {
       #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif

        Application.Quit();
    }

    #endregion

    #region Menu functions

    private void SetMenuButtons()
    {
        foreach (Button button in _menuButtons)
            button.onClick.RemoveAllListeners();

        _menuButtons[0].onClick.AddListener(PlayGame); //Play button
        _menuButtons[1].onClick.AddListener(OptionsGame); //Options button
        _menuButtons[2].onClick.AddListener(CreditsGame); //Credits button
        _menuButtons[3].onClick.AddListener(QuitGame); //Quit button
    }

    private void SetBackMenuButtons()
    {
        foreach(Button button in _backMenuButtons)
            button.onClick.RemoveAllListeners();

        _backMenuButtons[0].onClick.AddListener(BackOptions); //Options back Button
        _backMenuButtons[1].onClick.AddListener(BackCredits); //Credits back button
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != null)
            _lastButtonSelected = EventSystem.current.currentSelectedGameObject;

        if (EventSystem.current.currentSelectedGameObject == null)
            EventSystem.current.SetSelectedGameObject(_lastButtonSelected);

        PressedSelectedButton();
        ChangeSettingsPanel();
    }

    void PressedSelectedButton()
    {
        if (GameManager.instance.player.GetButtonDown("PressButton"))
        {
            EventSystem.current.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
        }
    }

    void ChangeSettingsPanel() 
    {
        if(_optionsPanel.gameObject.activeSelf && (GameManager.instance.player.GetButtonDown("PreviousSettingsPanel") || GameManager.instance.player.GetButtonDown("NextSettingsPanel")))
        {
            _settingsPanels[_indexSettingsPanel].SetActive(false);

            if (GameManager.instance.player.GetButtonDown("PreviousSettingsPanel"))
            {
                if (_indexSettingsPanel == 0)
                    _indexSettingsPanel = 1;
                else
                    _indexSettingsPanel = 0;
            }
            else if (GameManager.instance.player.GetButtonDown("NextSettingsPanel"))
            {
                if (_indexSettingsPanel == 1)
                    _indexSettingsPanel = 0;
                else
                    _indexSettingsPanel = 1;
            }

            _settingsPanels[_indexSettingsPanel].SetActive(true);

            GameObject currentSelected;

            Navigation navigation = new Navigation();

            navigation.mode = Navigation.Mode.Explicit;

            if (_indexSettingsPanel == 0)
            {
                _titleSettings.text = _titleGraphics;
                currentSelected = GetComponent<GraphicSettings_Script>().firstSelectedObject;
                navigation.selectOnDown = currentSelected.GetComponent<Button>();
            }
            else
            {
                _titleSettings.text = _titleAudio;
                currentSelected = GetComponent<MainMenu_Audio>().firstSelectedObject;
                navigation.selectOnDown = currentSelected.GetComponent<Slider>();
            }

            _backMenuButtons[0].navigation = navigation;

            EventSystem.current.SetSelectedGameObject(currentSelected);
        }
    }
    #endregion
}
