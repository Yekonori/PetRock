using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZoneScript : MonoBehaviour
{
    [SerializeField] bool finalZone;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            PlayerParameters.Instance.inSafeZone = true;
            PlayerParameters.Instance.playerStates = PlayerParameters.PlayerStates.Regular;

            if(finalZone)
            {
                Debug.Log("C'est la final Zone !!");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            PlayerParameters.Instance.inSafeZone = false;
        }
    }
}
