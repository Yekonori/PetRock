using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggers : MonoBehaviour
{
    private const string _safeZoneTag = "SafeZone";
    private const string _rockBalancingZoneTag = "RockBalancing";

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case _safeZoneTag:
                other.gameObject.GetComponent<SafeZoneScript>().EnterOnSafeZone();
                break;

            case _rockBalancingZoneTag:
                break;

            default:
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == _safeZoneTag)
            other.gameObject.GetComponent<SafeZoneScript>().ExitSafeZone();
    }
}
