using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerInput : MonoBehaviour
{
    Player player;
    [SerializeField] PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(0);

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

    // Update is called once per frame
    void Update()
    {
        float move = player.GetAxis("MoveVertical");
        float rotate = player.GetAxis("Rotate");

        playerMovement.SetMovementDirection(move, rotate);
    }
}
