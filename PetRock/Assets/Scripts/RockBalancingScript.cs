using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class RockBalancingScript : MonoBehaviour
{
    [HideInInspector]
    public Player player;
    [Range(0f, 0.5f)] float deadZone = 0.01f;

    // direction and rotation
    private int _moveForward = 0;
    private int _moveRotation = 0;

    //Speed rock
    [Header("Speed rock movement")]
    [SerializeField, Min(0f)] float speed = 5f;
    [SerializeField, Min(0f)] float rotationSpeed = 90f;

    [Header("Start rock position")]
    [SerializeField]
    private Transform startPosRock;

    [Header("Validate position rock")]
    [SerializeField]
    private Transform finalPosRock;

    [Header("RockPrefab")]
    [SerializeField]
    private GameObject prefabRock;
    private GameObject theRock;

    private bool canRockBalance = false;

    private void OnEnable()
    {
        player = GameManager.instance.player;
        theRock = Instantiate(prefabRock, startPosRock.position, Quaternion.identity, startPosRock.parent);
        StartCoroutine(startRockBalance());
    }

    // Update is called once per frame
    void Update()
    {
        if (!canRockBalance)
            return;

        float move = player.GetAxis("MovePetRock");
        int moveInt = Mathf.Abs(move) > deadZone ? (int)Mathf.Sign(move) : 0;

        float rotate = player.GetAxis("TiltPetRock");
        int rotateInt = Mathf.Abs(rotate) > deadZone ? (int)Mathf.Sign(rotate) : 0;

        SetMovementDirection(moveInt, rotateInt);

        theRock.transform.position += _moveForward * transform.forward * speed * Time.deltaTime; // += gravity * 
        theRock.transform.Rotate(Vector3.up, _moveRotation * rotationSpeed * Time.deltaTime);

        if (ValidatePos() && player.GetButton("ValidatePetRockPos"))
        {
            Debug.LogError("here");
        }
    }

    private void SetMovementDirection(float vertical, float rotate)
    {
        _moveForward = (int)vertical;
        _moveRotation = (int)rotate;
    }

    private bool ValidatePos()
    {
        return theRock.transform.position == finalPosRock.position && 
            theRock.transform.rotation == finalPosRock.rotation;
    }

    private IEnumerator startRockBalance()
    {
        yield return new WaitForSeconds(1);
        canRockBalance = true;
    }
}
