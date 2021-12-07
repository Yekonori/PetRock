using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioType : MonoBehaviour
{
    [HideInInspector]
    public AudioSource audioSource;
    [HideInInspector]
    public float maxVolume;

    public enum enumAudioType
    {
        Music,
        SFX
    }

    public enumAudioType audioType; 
    
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        maxVolume = audioSource.volume;

        if (audioType == enumAudioType.SFX)
        {
            AudioManager.instance._sfxAudios.Add(this);
        }
        else
        {
            AudioManager.instance._musicAudios.Add(this);
        }
    }

    public void ChangeVolume(float value)
    {
        audioSource.volume = value * maxVolume;
    }
}
