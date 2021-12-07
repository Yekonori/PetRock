using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public List<AudioType> _sfxAudios;
    public List<AudioType> _musicAudios;

    private float _globalVolume;
    private float _musicVolume;
    private float _sfxVolume;

    MainMenu_Audio _audioMainMenu;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }

        SetVolumes();
        //SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //Call when a scene is loaded
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ChangeGlobalVolume();
        ChangeSfxVolume();
        ChangeMusicVolume();
    }

    void SetVolumes()
    {
        _globalVolume = PlayerPrefs.GetFloat("GlobalVolume",  100.0f);
        _sfxVolume = PlayerPrefs.GetFloat("SfxVolume",  100.0f);
        _musicVolume = PlayerPrefs.GetFloat("MusicVolume",  100.0f);
    }

    public void SetGlobalVolume(Slider globalSlider)
    {
        _globalVolume = globalSlider.value;
        ChangeGlobalVolume();
    }

    public void SetSfxVolume(Slider sfxSlider)
    {
        _sfxVolume = sfxSlider.value;
        ChangeSfxVolume();
    }

    public void SetMusicVolume(Slider musicSlider)
    {
        _musicVolume = musicSlider.value;
        ChangeMusicVolume();
    }

    void ChangeSfxVolume()
    {
        foreach (AudioType sfx in _sfxAudios)
        {
            sfx.ChangeVolume(_sfxVolume / 100.0f);
        }
        PlayerPrefs.SetFloat("SfxVolume", _sfxVolume);
    }

    void ChangeMusicVolume()
    {
        foreach (AudioType music in _musicAudios)
        {
            music.ChangeVolume(_musicVolume / 100.0f);
        }
        PlayerPrefs.SetFloat("MusicVolume", _musicVolume);
    }

    void ChangeGlobalVolume()
    {
        AudioListener.volume = _globalVolume / 100.0f;
        PlayerPrefs.SetFloat("GlobalVolume", _globalVolume);
    }
}
