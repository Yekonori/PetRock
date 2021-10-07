using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockScript : MonoBehaviour
{
    public Transform player;
    public float distanceDetection = 2f;

    private bool hasPlayer = false;
    private bool beingCarried = false;




    // Update is called once per frame
    void Update()
    {
        //Calcule la distance entre Pierre et joueur
        float dist = Vector3.Distance(gameObject.transform.position, player.position);


        //Distance de ramassage
        if (dist <= distanceDetection)
        {
            hasPlayer = true;
        }
        else
        {
            hasPlayer = false;
        }

        // si on peut ramasser et qu'on appuie sur A
        if (hasPlayer && Input.GetKeyDown(KeyCode.A))
        {
            if (!beingCarried)
            {
                GetComponent<Rigidbody>().isKinematic = true;
                transform.parent = player;
                beingCarried = true;
            }
            else
            {
                GetComponent<Rigidbody>().isKinematic = false;
                transform.parent = null;
                beingCarried = false;
            }
        }

        if (beingCarried)
        {
            transform.position = player.transform.position;

        }

    }
}
