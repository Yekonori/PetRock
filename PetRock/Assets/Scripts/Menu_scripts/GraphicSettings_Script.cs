using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GraphicSettings_Script : MonoBehaviour
{
    [Header("Graphics settings buttons")]
    [SerializeField]
    private List<Button> _graphicSettingsButtons = new List<Button>();

    [Header("Resolution")]
    [SerializeField]
    private TextMeshProUGUI _resolutonText;
    private int[] widthRes = new int[] { 720, 1280, 1600, 1920 };
    private int[] heightRes = new int[] { 480, 720, 900, 1080 };
    private int _indexRes;

    [Header("Windows Mode")]
    [SerializeField]
    private TextMeshProUGUI _windowsModeText;
    private FullScreenMode[] _screenModes = new FullScreenMode[] { FullScreenMode.ExclusiveFullScreen, FullScreenMode.FullScreenWindow, FullScreenMode.Windowed };
    private string[] _screenModesText = new string[] { "FULLSCREEN", "BORDERLESS", "WINDOWED" };
    private int _indexWindowsMode;

    [Header("Quality levels")]
    [SerializeField]
    private TextMeshProUGUI _qualityFxTextValue;
    private string[] _qualityFxTexts = new string[] { "LOW", "NORMAL", "HIGH", "ULTRA" };
    private int _indexEffets;

    // Start is called before the first frame update
    void Awake()
    {
        InitGraphicsSettings();
    }

    void InitGraphicsSettings()
    {
        //Quality levels
        _indexEffets = PlayerPrefs.GetInt("QualityFx", 3);
        QualitySettings.SetQualityLevel(_indexEffets);
        _qualityFxTextValue.text = _qualityFxTexts[_indexEffets];

        //Windows mode
        _indexWindowsMode = PlayerPrefs.GetInt("WindowsMode", 0);
        _windowsModeText.text = _screenModesText[_indexWindowsMode];

        //Resolution
        _indexRes = PlayerPrefs.GetInt("Resolution", 3);
        Screen.SetResolution(widthRes[_indexRes], heightRes[_indexRes], _screenModes[_indexWindowsMode]);
        _resolutonText.text = widthRes[_indexRes] + " x " + heightRes[_indexRes];

        SetGraphicsSettingsButtons();
    }

    void SetGraphicsSettingsButtons()
    {
        foreach (Button button in _graphicSettingsButtons)
            button.onClick.RemoveAllListeners();

        _graphicSettingsButtons[0].onClick.AddListener(ChangeResolution); //Resolution button
        _graphicSettingsButtons[1].onClick.AddListener(ChangeWindowMode); //Windows mode button
        _graphicSettingsButtons[2].onClick.AddListener(ChangeQualityFx); //Quality levels button
    }

    void ChangeResolution()
    {
        Debug.LogError("here");

        if (_indexRes == 3)
            _indexRes = 0;
        else
            _indexRes++;

        _resolutonText.text = widthRes[_indexRes] + " x " + heightRes[_indexRes];
        Screen.SetResolution(widthRes[_indexRes], heightRes[_indexRes], _screenModes[_indexWindowsMode]);

        PlayerPrefs.SetInt("Resolution", _indexRes);
    }

    void ChangeWindowMode()
    {
        if (_indexWindowsMode == 2)
            _indexWindowsMode = 0;
        else
            _indexWindowsMode++;

        _windowsModeText.text = _screenModesText[_indexWindowsMode];
        Screen.SetResolution(widthRes[_indexRes], heightRes[_indexRes], _screenModes[_indexWindowsMode]);

        PlayerPrefs.SetInt("WindowsMode", _indexWindowsMode);
    }

    void ChangeQualityFx()
    {

        if (_indexEffets == 3)
            _indexEffets = 0;
        else
            _indexEffets++;

        QualitySettings.SetQualityLevel(_indexEffets);
        _qualityFxTextValue.text = _qualityFxTexts[_indexEffets];

        PlayerPrefs.SetInt("QualityFx", _indexEffets);
    }
}
