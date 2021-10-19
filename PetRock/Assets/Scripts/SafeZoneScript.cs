using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZoneScript : MonoBehaviour
{
    [SerializeField] bool finalZone;

    PlayerParameters _playerParameters;

    private void Start()
    {
        _playerParameters = PlayerParameters.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            _playerParameters.UpdateStateSafeZone(true);
            _playerParameters.UpdatePlayerState(PlayerParameters.PreviousPlayerStates.Regular, PlayerParameters.PlayerStates.Regular);

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
            _playerParameters.UpdateStateSafeZone(false);
        }
    }
}
