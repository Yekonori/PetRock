using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerTriggers : MonoBehaviour
{
    private const string _safeZoneTag = "SafeZone";
    private const string _rockBalancingZoneTag = "RockBalancing";
    private Animator anim;

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case _safeZoneTag:
                other.gameObject.GetComponent<SafeZoneScript>().EnterOnSafeZone();
                break;

            case _rockBalancingZoneTag:
                StartCoroutine(GameManager.instance.startRB(other.gameObject));
                anim = this.GetComponent<Animator>();
                anim.SetBool("rockBalance", true);
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
