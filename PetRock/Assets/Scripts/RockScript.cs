using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockScript : MonoBehaviour
{
    private bool onPlayer = true;

    [Header("Up and Down")]
    [SerializeField, Min(0)] float maxYmovement = 0.1f;
    [SerializeField, Min(0)] float yMovementTime = 3;
    private float yCurrent = 0;
    private float yTimer = 0;
    private float omega = 0;

    [Header("Rotation")]
    [SerializeField, Min(0)] float rotationSpeed = 10f;

    [Header("MoveToGround")]
    [SerializeField] float timeToMove = 1.5f;
    private float moveTimer = 0;
    private bool moving = false;
    private Vector3 startPos;
    private Transform targetPos;

    private void Start()
    {
        omega = Mathf.PI * 2 / yMovementTime;
        ResetPosition();
    }

    private void Update()
    {
        if (onPlayer)
        {
            yTimer += Time.deltaTime;
            yCurrent = maxYmovement * Mathf.Sin(yTimer * omega);
            transform.localPosition = new Vector3(0, yCurrent, 0);

            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
        else if (moving)
        {
            moveTimer += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, targetPos.position, moveTimer/timeToMove);
            if (moveTimer > timeToMove)
            {
                transform.position = targetPos.position;
                moving = false;
            }
        }
    }

    public void ResetPosition()
    {
        onPlayer = true;
        transform.localPosition = Vector3.zero;
        yCurrent = 0;
        yTimer = 0;
    }

    public void MoveTotarget(Transform target)
    {
        moveTimer = 0;
        startPos = transform.position;
        moving = true;
        onPlayer = false;
        targetPos = target;
    }
}
