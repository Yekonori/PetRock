using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerSpotState
{
    NotSpoted,
    Spoted,
    Resuming
}

public class EyeMovement : MonoBehaviour
{
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private EyeParam param;
    private float moveTimer = 0;
    private float rotateTimer = 0;
    private float startAngle;

    [Header("Spot Player")]
    [SerializeField] Transform player;
    [SerializeField] float rotateTowardsPlayerSpeed = 90f;
    private PlayerSpotState spotState = PlayerSpotState.NotSpoted;
    private Quaternion rot;
    private float resumeTimer = 0f;

    private void Start()
    {
        startAngle = transform.eulerAngles.y;
        if (param.movementType == MovementType.Moving)
        {
            transform.position = param.startPos.position;
            transform.rotation = param.startPos.rotation;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (spotState)
        {
            case PlayerSpotState.NotSpoted:
                if (param.movementType == MovementType.Moving)
                {
                    moveTimer += Time.deltaTime;
                    if (moveTimer > param.timeToMove) moveTimer = 0;
                    transform.position = Vector3.Lerp(param.startPos.position, param.endPos.position, curve.Evaluate(moveTimer / param.timeToMove));
                }

                switch (param.rotationType)
                {
                    case RotationType.NoRotation:
                        break;

                    case RotationType.EssuieGlace:
                        rotateTimer += Time.deltaTime;
                        if (rotateTimer > param.timeToRotate) rotateTimer = 0;
                        float angle = Mathf.Lerp(-param.maxAngle, param.maxAngle, curve.Evaluate(rotateTimer / param.timeToRotate));
                        transform.rotation = Quaternion.Euler(0, startAngle + angle, 0);
                        break;

                    case RotationType.FullRotation:
                        transform.Rotate(Vector3.up, param.rotationSpeed * Time.deltaTime);
                        break;
                }
                break;

            case PlayerSpotState.Spoted:
                Quaternion targetRotation = Quaternion.LookRotation(player.position - transform.position, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotateTowardsPlayerSpeed * Time.deltaTime);
                break;

            case PlayerSpotState.Resuming:
                if (param.rotationType == RotationType.EssuieGlace)
                {
                    resumeTimer += Time.deltaTime;
                    float angle = Mathf.Lerp(-param.maxAngle, param.maxAngle, curve.Evaluate(rotateTimer / param.timeToRotate));
                    Quaternion targRotation = Quaternion.Euler(0, startAngle + angle, 0);
                    transform.rotation = Quaternion.Lerp(rot, targRotation, resumeTimer);
                }
                break;
        }
    }

    public void SetPlayerSpotted(PlayerSpotState value, float timer = 0f)
    {
        spotState = value;
        resumeTimer = timer;
        if (spotState == PlayerSpotState.Resuming && timer == 0f)
        {
            rot = transform.rotation;
        }
    }
}
