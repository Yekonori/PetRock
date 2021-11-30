using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DialogueStress
{
    public float panicLevel = 25;
    public DialogueScript conversation;
    [HideInInspector] public bool canActivate = true;
    [HideInInspector] public bool needActivate = false;
    [HideInInspector] public float timer = 0;
}

public class GetSpot_Dialog : MonoBehaviour
{
    [SerializeField] DialogDisplay dialogDisplay;

    [Header ("Is On Giant Zone")]
    [SerializeField] DialogueScript conversation;
    [SerializeField] float timeBetweenSpotedDialog = 30;
    private float timerSpotedDialog = 0;
    private bool canDisplaySpotedDialog = true;

    [Header("Is On Stress")]
    [SerializeField] float timeBetweenSameStressDialogue = 30;
    [SerializeField] List<DialogueStress> listDialogueStress;
    private float previousPanic = 0f;

    PlayerParameters _playerParameters;

    private void Start()
    {
        _playerParameters = PlayerParameters.Instance;
    }

    void Update()
    {
        if (_playerParameters.IsOnTimeOut())
            return;

        if (_playerParameters.HasBeenOnGiantZone() && !dialogDisplay.activeDialog)
        {
            if (canDisplaySpotedDialog)
            {
                dialogDisplay.SetThisDialogTex(conversation);
                dialogDisplay.DisplayDialog();
                dialogDisplay.NextDialog();
                canDisplaySpotedDialog = false;
            }
            else
            {
                timerSpotedDialog += Time.deltaTime;
                if (timerSpotedDialog > timeBetweenSpotedDialog)
                {
                    timerSpotedDialog = 0;
                    canDisplaySpotedDialog = true;
                }
            }
        }

        float currentPanic = _playerParameters.panicGauge;

        foreach(DialogueStress dialogueStress in listDialogueStress)
        {
            if (!dialogueStress.canActivate)
            {
                dialogueStress.timer += Time.deltaTime;
                if (dialogueStress.timer > timeBetweenSameStressDialogue)
                {
                    dialogueStress.timer = 0;
                    dialogueStress.canActivate = true;
                }
            }

            if (previousPanic < dialogueStress.panicLevel && currentPanic > dialogueStress.panicLevel && dialogueStress.canActivate)
            {
                dialogueStress.needActivate = true;
            }

            if (dialogueStress.needActivate && !dialogDisplay.activeDialog)
            {
                dialogDisplay.SetThisDialogTex(dialogueStress.conversation);
                dialogDisplay.DisplayDialog();
                dialogDisplay.NextDialog();
                dialogueStress.needActivate = false;
                dialogueStress.canActivate = false;
            }
        }

        previousPanic = _playerParameters.panicGauge;
    }
}
