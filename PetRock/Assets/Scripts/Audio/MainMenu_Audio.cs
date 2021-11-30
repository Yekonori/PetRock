using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenu_Audio : MonoBehaviour
{
    [Header("First selected object")]
    public GameObject firstSelectedObject;

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
    }

    private void Start()
    {
        InitSlider(_globalSlider, _globalVolumeText, "GlobalVolume");
        InitSlider(_sfxSlider, _sfxVolumeText, "SfxVolume");
        InitSlider(_musicSlider, _musicVolumeText, "MusicVolume");
    }

    void InitSlider(Slider slider, TextMeshProUGUI text, string PlayerPrefKey)
    {
        slider.value = PlayerPrefs.GetFloat(PlayerPrefKey, 100.0f);
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
