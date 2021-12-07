using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Transition_Script : MonoBehaviour
{
    public CanvasGroup transitionCanvas;
    [HideInInspector]
    public bool isOnTransition = false;

    [SerializeField]
    private TextMeshProUGUI _textTransition;
    [SerializeField, TextArea]
    private string _textSceneTransition;

    private void Awake()
    {
        _textTransition.text = _textSceneTransition;
    }

    private void Update()
    {
        if (transitionCanvas.alpha <= 0.1f)
            isOnTransition = false;
        else
            isOnTransition = true;
    }
}
