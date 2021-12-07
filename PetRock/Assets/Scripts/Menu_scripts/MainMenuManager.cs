using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
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

    [Header("Input settings")]
    [SerializeField]
    private TextMeshProUGUI settingsNextPanel;
    [SerializeField]
    private TextMeshProUGUI settingsPreviousPanel;

    private const string LB = "LB";
    private const string RB = "RB";

    private const string L1 = "L1";
    private const string R1 = "R1";

    private const string L = "L";
    private const string R = "R";

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

    [Header("Cinematic")]
    [SerializeField]
    private PlayableDirector _introCinematic;
    private bool _inCinematic = false;

    [Header("Sounds")]
    [SerializeField]
    private AudioClip _launchClip;

    GameManager _gameManager;

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

        _gameManager = GameManager.instance;

        _gameManager.inMainMenu = true;

        _backgroundMainMenu = GetComponent<Image>();
        _menuPanels = GetComponent<CanvasGroup>();

        _titleSettings.text = _titleGraphics;

        _introCinematic.played += Director_Played;
        _introCinematic.stopped += Director_Stopped;
    }

    private void Start()
    {
        _gameManager.dofPostProcessing.focusDistance.value = 0.1f;

        SetMenuButtons();
        SetBackMenuButtons();

        GetController();
    }

    private void Director_Played(PlayableDirector obj)
    {
        _inCinematic = true;
        EventSystem.current.SetSelectedGameObject(null);
    }

    private void Director_Stopped(PlayableDirector obj)
    {
        StartCoroutine(StartGame());
    }

    #region Buttons function

    void PlayGame(Button_Script button)
    {
        if (_inCinematic)
            return;

        button.GetAudioSource().PlayOneShot(_launchClip);

        DOTween.To(() => _gameManager.dofPostProcessing.focusDistance.value, x => _gameManager.dofPostProcessing.focusDistance.value = x, 10.0f, 5.0f).OnPlay(() => 
        { 
            _menuPanels.DOFade(0, 1).OnComplete(()=> _introCinematic.Play()); 
        });
    }

    IEnumerator StartGame()
    {
        yield return new WaitUntil(() => _playerStartAnimator.GetCurrentAnimatorStateInfo(0).IsName("StandingIdle"));

        GetComponent<StartDialogue_Script>().StartDialogue();

        _cam.ChangeTargetCam(_rock);

        yield return new WaitWhile(() => GetComponent<StartDialogue_Script>().CheckEndDialogue());

        _gameManager.inMainMenu = false;

        _tutoCanvas.SetActive(true);
        _tutoCanvas.GetComponent<Tutorial_Script>().IsFirstPlayerMovement();

        Destroy(gameObject);
    }

    void OptionsGame(Button_Script button)
    {
        if (_inCinematic)
            return;

        PanelSwitch(_mainMenuPanel, _optionsPanel, button);
    }

    void CreditsGame(Button_Script button)
    {
        if (_inCinematic)
            return;

        PanelSwitch(_mainMenuPanel, _creditsPanel, button);
        _backgroundMainMenu.DOFade(1, 1).OnComplete(() => _creditsAnim.SetBool("PlayCredits", true));
    }

    void BackCredits(Button_Script button)
    {
        if (_inCinematic)
            return;

        PanelSwitch(_creditsPanel, _mainMenuPanel, button);
        _backgroundMainMenu.DOFade(0, 1);
    }

    void BackOptions(Button_Script button)
    {
        if (_inCinematic)
            return;

        PanelSwitch(_optionsPanel, _mainMenuPanel, button);
    }

    void PanelSwitch(CanvasGroup firstPanel, CanvasGroup secondPanel, Button_Script button)
    {
        button.GetAudioSource().Play();
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

        _menuButtons[0].onClick.AddListener(() => PlayGame(_menuButtons[0].gameObject.GetComponent<Button_Script>())); //Play button
        _menuButtons[1].onClick.AddListener(() => OptionsGame(_menuButtons[1].gameObject.GetComponent<Button_Script>())); //Options button
        _menuButtons[2].onClick.AddListener(() => CreditsGame(_menuButtons[2].gameObject.GetComponent<Button_Script>())); //Credits button
        _menuButtons[3].onClick.AddListener(QuitGame); //Quit button
    }

    private void SetBackMenuButtons()
    {
        foreach(Button button in _backMenuButtons)
            button.onClick.RemoveAllListeners();

        _backMenuButtons[0].onClick.AddListener(() => BackOptions(_backMenuButtons[0].gameObject.GetComponent<Button_Script>())); //Options back Button
        _backMenuButtons[1].onClick.AddListener(() => BackCredits(_backMenuButtons[1].gameObject.GetComponent<Button_Script>())); //Credits back button
    }

    private void Update()
    {
        if (!_inCinematic)
        {
            if (EventSystem.current.currentSelectedGameObject != null)
                _lastButtonSelected = EventSystem.current.currentSelectedGameObject;

            if (EventSystem.current.currentSelectedGameObject == null)
                EventSystem.current.SetSelectedGameObject(_lastButtonSelected);

            PressedSelectedButton();
            ChangeSettingsPanel();
        }
    }

    void PressedSelectedButton()
    {
        if (_gameManager.player.GetButtonDown("PressButton"))
        {
            EventSystem.current.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
        }
    }

    void GetController()
    {
        switch (ControllerType.Instance.typeController)
        {
            case ControllerType.TypeController.XBox:
                settingsNextPanel.text = RB;
                settingsPreviousPanel.text = LB;
                break;
            case ControllerType.TypeController.Playstation:
                settingsNextPanel.text = R1;
                settingsPreviousPanel.text = L1;
                break;
            case ControllerType.TypeController.Switch:
                settingsNextPanel.text = R;
                settingsPreviousPanel.text = L;
                break;
            default:
                settingsNextPanel.text = RB;
                settingsPreviousPanel.text = LB;
                break;
        }
    }

    void ChangeSettingsPanel() 
    {
        if(_optionsPanel.gameObject.activeSelf && (_gameManager.player.GetButtonDown("PreviousSettingsPanel") || _gameManager.player.GetButtonDown("NextSettingsPanel")))
        {
            _settingsPanels[_indexSettingsPanel].SetActive(false);

            if (_gameManager.player.GetButtonDown("PreviousSettingsPanel"))
            {
                if (_indexSettingsPanel == 0)
                    _indexSettingsPanel = 1;
                else
                    _indexSettingsPanel = 0;
            }
            else if (_gameManager.player.GetButtonDown("NextSettingsPanel"))
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
                navigation.selectOnUp = GetComponent<GraphicSettings_Script>().ObjectOnUp;
            }
            else
            {
                _titleSettings.text = _titleAudio;
                currentSelected = GetComponent<MainMenu_Audio>().firstSelectedObject;
                navigation.selectOnUp = GetComponent<MainMenu_Audio>().ObjectOnUp;
            }

            _backMenuButtons[0].navigation = navigation;

            EventSystem.current.SetSelectedGameObject(currentSelected);
        }
    }
    #endregion
}
