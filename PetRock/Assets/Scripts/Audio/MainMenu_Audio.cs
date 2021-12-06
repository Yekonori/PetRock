using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenu_Audio : MonoBehaviour
{
    [Header("Selected object")]
    public GameObject firstSelectedObject;
    public Selectable ObjectOnUp;

    [Header("Slider")]
    [SerializeField]
    private Slider _globalSlider;
    [SerializeField]
    private Slider _sfxSlider;
    [SerializeField]
    private Slider _musicSlider;

    [Header("Text")]
    [SerializeField]
    private TextMeshProUGUI _globalVolumeText;
    [SerializeField]
    private TextMeshProUGUI _musicVolumeText;
    [SerializeField]
    private TextMeshProUGUI _sfxVolumeText;

    private float startGlobalValue;
    private float startSfxValue;
    private float startMusicValue;

    public static MainMenu_Audio instance;

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

        startGlobalValue = PlayerPrefs.GetFloat("GlobalVolume", 100.0f);
        startSfxValue = PlayerPrefs.GetFloat("SfxVolume", 100.0f);
        startMusicValue = PlayerPrefs.GetFloat("MusicVolume", 100.0f);
    }

    private void Start()
    {
        InitSlider(_globalSlider, _globalVolumeText, startGlobalValue);
        InitSlider(_sfxSlider, _sfxVolumeText, startSfxValue);
        InitSlider(_musicSlider, _musicVolumeText, startMusicValue);
    }

    void InitSlider(Slider slider, TextMeshProUGUI text, float value)
    {
        slider.value = value;
        text.text = ((int)(slider.value)).ToString();
    }

    public void ChangeGlobalVolume()
    {
        AudioManager.instance.SetGlobalVolume(_globalSlider);
        _globalVolumeText.text = ((int)(_globalSlider.value)).ToString();
    }

    public void ChangeSfxVolume()
    {
        AudioManager.instance.SetSfxVolume(_sfxSlider);
        _sfxVolumeText.text = ((int)(_sfxSlider.value)).ToString();
    }

    public void ChangeMusicVolume()
    {
        AudioManager.instance.SetMusicVolume(_musicSlider);
        _musicVolumeText.text = ((int)(_musicSlider.value)).ToString();
    }
}
