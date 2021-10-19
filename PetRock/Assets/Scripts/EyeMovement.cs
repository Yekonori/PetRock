using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeMovement : MonoBehaviour
{
    private AnimationCurve curve;
    private EyeParam param;
    private float moveTimer = 0;
    private float rotateTimer = 0;
    private float startAngle;

    public void Init(EyeParam _param, AnimationCurve animationCurve)
    {
        curve = animationCurve;
        param = _param;
        transform.position = param.startPos.position;
        transform.rotation = param.startPos.rotation;
        startAngle = transform.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
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
    }
}
