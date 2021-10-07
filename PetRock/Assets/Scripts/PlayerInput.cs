using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerInput : MonoBehaviour
{
    #region Script Parameters
    
    [SerializeField] PlayerMovement playerMovement;

    #endregion Script Parameters

    #region Fields
    
    private Player _player;

    #endregion Fields

    void Start()
    {
        _player = ReInput.players.GetPlayer(0);

        if (playerMovement == null)
        {
            playerMovement = GetComponentInChildren<PlayerMovement>();
            if (playerMovement == null)
            {
                Debug.LogError("No PlayerMovement found in scene.");
                Application.Quit();
            }
        }
    }

    void Update()
    {
        float move = _player.GetAxis("MoveVertical");
        float rotate = _player.GetAxis("Rotate");

        playerMovement.SetMovementDirection(move, rotate);
    }
}
