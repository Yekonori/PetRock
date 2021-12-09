using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;

public class Button_Script : MonoBehaviour
{
    private bool _isAlreadySelected = false;
    private AudioSource _audioSource;

    [SerializeField]
    private AudioClip _selectedClip;

    private Selectable selectable;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        selectable = GetComponent<Selectable>();

        if (EventSystem.current.firstSelectedGameObject == gameObject)
            IsSelected();
        else
            NotSelected();
    }

    private void Start()
    {
        if(_audioSource.mute)
            _audioSource.mute = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            if (!_isAlreadySelected)
                IsSelected();
        }
        else
        {
            if (_isAlreadySelected)
                NotSelected();
        }
    }

    void NotSelected()
    {
        _isAlreadySelected = false;

        selectable.targetGraphic.color = selectable.colors.normalColor;

        if (GetComponentInChildren<TextMeshProUGUI>() != null)
            GetComponentInChildren<TextMeshProUGUI>().color = new Color(1, 1, 1, 0.5f);
    }

    void IsSelected()
    {
        _isAlreadySelected = true;

        selectable.targetGraphic.color = selectable.colors.selectedColor;

        if (GetComponentInChildren<TextMeshProUGUI>() != null)
            GetComponentInChildren<TextMeshProUGUI>().color = Color.white;

        if (_audioSource != null)
        {
            _audioSource.PlayOneShot(_selectedClip);
        }
    }

    public AudioSource GetAudioSource()
    {
        return _audioSource;
    }
}
