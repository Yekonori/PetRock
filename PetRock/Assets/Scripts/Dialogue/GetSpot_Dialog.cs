using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetSpot_Dialog : MonoBehaviour
{
    public DialogDisplay dialogDisplay;

    [Header ("Is On Giant Zone")]
    public DialogueScript conversation1;

    [Header("Is On Stress")]
    public DialogueScript conversation2;
    

    PlayerParameters _playerParameters;
    private int etat;

    void Start()
    {
        _playerParameters = PlayerParameters.Instance;
        etat = 0;
    }

    void Update()
    {
        if (_playerParameters.HasBeenOnGiantZone() && !dialogDisplay.activeDialog)
        {
            dialogDisplay.SetThisDialogTex(conversation1);
            dialogDisplay.DisplayDialog();
            dialogDisplay.NextDialog();
            etat = 1;
        }
        
        if(_playerParameters.HasBeenOnStressed() && !dialogDisplay.activeDialog && etat != 2)
        {
            dialogDisplay.SetThisDialogTex(conversation2);
            dialogDisplay.DisplayDialog();
            dialogDisplay.NextDialog();
            etat = 2;
        }



    }

}
