using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDialogue_Script : MonoBehaviour
{
    [SerializeField]
    private DialogueScript _startGameDialogue;

    [SerializeField]
    private DialogDisplay _dialogDisplay;

    public void StartDialogue()
    {
        _dialogDisplay.SetThisDialogTex(_startGameDialogue);
        _dialogDisplay.DisplayDialog();
        _dialogDisplay.NextDialog();
    }

    public bool CheckEndDialogue()
    {
        return _dialogDisplay.activeDialog;
    }
}
