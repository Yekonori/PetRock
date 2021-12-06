using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Transition_Script : MonoBehaviour
{
    public CanvasGroup transitionCanvas;

    [SerializeField]
    private TextMeshProUGUI _textTransition;
    [SerializeField, TextArea]
    private string _textSceneTransition;

    private void Awake()
    {
        _textTransition.text = _textSceneTransition;
    }
}
