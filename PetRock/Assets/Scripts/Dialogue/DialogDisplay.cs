using System;
using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

public class DialogDisplay : MonoBehaviour
{
    public DialogueScript conversation;

    public GameObject speakerLeft;
    public GameObject speakerRight;

    public float textDuration = 1;

    public float timeBeforeNextPanel = 2;

    public PlayerMovement player;

    private int activeLineIndex = 0;
   
    private DialogueUI speakerUILeft;
    private DialogueUI speakerUIRight;

    public bool activeDialog;
    private float t = 0;

    private Player _player;

    private bool canNextDialogue = true;
    private IEnumerator currentRoutine = null;
    private List<Action> actionWhenDialogEnd = new List<Action>();

    private bool needToCompleteDialogue = false;
    private DialogueUI currentSpeakerUI;
    private string currentTextSpeaking;

    // Camera Movement
    private FixedFollowView fixedFollowView;
    private Transform playerTransform;
    private Transform rockTransform;
    private Transform middleTransform;

    [Header("Sounds")]
    [SerializeField]
    private AudioClip _showDialogueSound;
    private AudioSource _audioSource;

    private void Start()
    {
        _player = ReInput.players.GetPlayer(0);

        _audioSource = GetComponent<AudioSource>();

        speakerUILeft = speakerLeft.GetComponent<DialogueUI>();
        speakerUIRight = speakerRight.GetComponent<DialogueUI>();

        speakerUILeft.Speaker = conversation.leftCharacter;
        speakerUIRight.Speaker = conversation.rightCharacter;

        speakerLeft.SetActive(false);
        speakerRight.SetActive(false);

        activeDialog = false;
    }

    private void Update()
    {
        if (conversation.automatic && canNextDialogue)
        {
            if(t >= timeBeforeNextPanel)
            {
                NextDialog();
                t = 0;
            }
            else
            {
                t += Time.deltaTime;
            }
        }
        else if (_player.GetButtonDown("ProgressDialogue") && !conversation.automatic && canNextDialogue)
        {
            if (needToCompleteDialogue)
            {
                if (currentRoutine != null)
                {
                    StopCoroutine(currentRoutine);
                    EndSpeaking();
                }
            }
            else
            {
                canNextDialogue = false;
                NextDialog();
            }
        }
    }

    //Passe au texte suivant
    public void NextDialog()
    {
        if(activeDialog)
            _audioSource.PlayOneShot(_showDialogueSound);

        if (activeLineIndex < conversation.lines.Length && activeDialog)
        {
            DisplayLine();
            activeLineIndex += 1;
        }
        else
        {
            speakerUILeft.Hide();
            speakerUIRight.Hide();

            activeDialog = false;
            activeLineIndex = 0;

            if (actionWhenDialogEnd.Count > 0)
            {
                foreach (Action action in actionWhenDialogEnd)
                {
                    action.Invoke();
                }
                actionWhenDialogEnd.Clear();
            }
        }
    }

    public void DisplayLine()
    {
        Line line = conversation.lines[activeLineIndex];
        Characters character = line.character;

        if (speakerUILeft.SpeakerIs(character))
        {
            SetDialog(speakerUILeft, speakerUIRight, line.text);
        }
        else
        {
            SetDialog(speakerUIRight, speakerUILeft, line.text);
        }

        if (line.focusCharacter)
        {
            FocusChar(line.character.fullName == "Me");
        }
        else if (fixedFollowView != null)
        {
            fixedFollowView.target = middleTransform;
        }
    }

    void SetDialog(DialogueUI activeSpeakerUI, DialogueUI inactiveSpeakerUI, string text)
    {
        activeSpeakerUI.Show();
        inactiveSpeakerUI.Hide();

        currentSpeakerUI = activeSpeakerUI;
        currentTextSpeaking = text;

        needToCompleteDialogue = false;
        currentRoutine = StartSpeaking(activeSpeakerUI, text);

        StartCoroutine(currentRoutine);
    }

    //Active la boîte de dialogue
    public void DisplayDialog()
    {
        activeDialog = true;
    }

    public void SetThisDialogTex(DialogueScript dialogText)
    {
        if (conversation != null && currentRoutine != null)
        {
            EndSpeaking();
            StopCoroutine(currentRoutine);
        }

        conversation = dialogText;
    }

    //Défilement du texte
    private IEnumerator StartSpeaking(DialogueUI active,  string text)
    {
        active.dialog.text = "";
        canNextDialogue = true;
        needToCompleteDialogue = true;

        int textLength = text.Length;
        float textSpeedRatio = textDuration / textLength;

        active.dialog.text = "";

        foreach (char character in text)
        {            
            active.dialog.text += character;

            if (active.dialog.text == text)
            {
                needToCompleteDialogue = false;
            }

            yield return new WaitForSeconds(textSpeedRatio);
        }
    }

    public void SetEndAction(Action action)
    {
        actionWhenDialogEnd.Add(action);
    }

    private void EndSpeaking()
    {
        currentSpeakerUI.dialog.text = currentTextSpeaking;
        needToCompleteDialogue = false;
    }

    void FocusChar(bool isPlayer)
    {
        fixedFollowView.target = isPlayer ? playerTransform : rockTransform;
    }

    public void SetView(FixedFollowView view, Transform player, Transform rock, Transform middle)
    {
        fixedFollowView = view;
        playerTransform = player;
        rockTransform = rock;
        middleTransform = middle;
    }
}
