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

    public float timeBeforeNextPanel = 3;

    private int activeLineIndex = 0;
   
    private DialogueUI speakerUILeft;
    private DialogueUI speakerUIRight;

    public bool activeDialog;
    private float t = 0;

    private Player _player;

    private bool canNextDialogue = true;

    private void Start()
    {
        _player = ReInput.players.GetPlayer(0);

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
        if (conversation.automatic)
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
            canNextDialogue = false;
            NextDialog();
        }
    }

    //Passe au texte suivant
    public void NextDialog()
    {
        if(activeLineIndex < conversation.lines.Length && activeDialog)
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
    }

    void SetDialog(DialogueUI activeSpeakerUI, DialogueUI inactiveSpeakerUI, string text)
    {
        activeSpeakerUI.Show();
        inactiveSpeakerUI.Hide();

        StartCoroutine(StartSpeaking(activeSpeakerUI, text));
    }

    //Active la boîte de dialogue
    public void DisplayDialog()
    {
        activeDialog = true;
    }

    public void SetThisDialogTex(DialogueScript dialogText)
    {
        conversation = dialogText;
    }



    //Défilement du texte
    private IEnumerator StartSpeaking(DialogueUI active,  string text)
    {
        active.dialog.text = "";
        canNextDialogue = true;

        int textLength = text.Length;
        float textSpeedRatio = textDuration / textLength;

        foreach (char character in text)
        {
            active.dialog.text += character;

            yield return new WaitForSeconds(textSpeedRatio);
        }
    }

}
