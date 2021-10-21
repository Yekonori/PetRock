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

    public void EnterOnSafeZone()
    {
        _playerParameters.UpdateStateSafeZone(true);
        _playerParameters.UpdatePlayerState(PlayerParameters.PreviousPlayerStates.Regular, PlayerParameters.PlayerStates.Regular);

        if (finalZone)
        {
            Debug.Log("C'est la final Zone !!");
        }
    }

    public void ExitSafeZone()
    {
        _playerParameters.UpdateStateSafeZone(false);
    }
}
