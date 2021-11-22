using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_Dialog : MonoBehaviour
{
    public DialogueScript conversation;
    public DialogDisplay dialogDisplay;

    [Header("Positions")]
    [SerializeField] Transform posPlayer;
    [SerializeField] Transform posRock;

    private TriggeredViewVolume volume;

    private void Start()
    {
        volume = GetComponent<TriggeredViewVolume>();
        volume.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            dialogDisplay.SetThisDialogTex(conversation);
            dialogDisplay.DisplayDialog();
            dialogDisplay.NextDialog();

            if (conversation.automatic)
            {
                Destroy(this.gameObject);
            }
            else
            {
                other.transform.position = posPlayer.position;
                other.transform.rotation = posPlayer.rotation;
                posRock.gameObject.SetActive(true);
                volume.ActiveView(true);
                //dialogDisplay.SetEndAction(() => volume.ActiveView(false));
                //dialogDisplay.SetEndAction(() => Destroy(this.gameObject));
            }
        }
    }
}
