using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerInput : MonoBehaviour
{
    #region Script Parameters
    
    [SerializeField] PlayerMovement playerMovement;
    [Range(0f, 0.5f)] float deadZone = 0.4f;

    #endregion Script Parameters

    #region Fields

    private Player _player;
    
    #endregion Fields

    void Start()
    {
        _player = ReInput.players.GetPlayer(0);

        GameManager.instance.player = _player;

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
        if (_player.GetButton("Pause"))
        {
            GameManager.instance.PauseGame();
        }

        if (GameManager.instance._inPause || GameManager.instance.inRockBalancing)
            return;

        playerMovement.SetMovementDirection(_player.GetAxis("DirX"), _player.GetAxis("DirY"));
    }
}
