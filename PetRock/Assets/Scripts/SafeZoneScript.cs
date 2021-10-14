using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZoneScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("Player enter");
            other.GetComponent<PlayerMovement>().SetSafe(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player out");
            other.GetComponent<PlayerMovement>().SetSafe(false); ;
        }
    }
}
