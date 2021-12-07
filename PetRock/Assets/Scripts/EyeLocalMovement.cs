using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeLocalMovement : MonoBehaviour
{
    [SerializeField, Min(0)] float maxYmovement = 0.1f;
    [SerializeField, Min(0)] float yMovementTime = 3;
    [SerializeField, Min(0)] float yOffset = 0;
    private float yCurrent = 0;
    private float yTimer = 0;
    private float omega = 0;
    private float yLocalPosStart = 0;

    // Start is called before the first frame update
    void Start()
    {
        omega = Mathf.PI * 2 / yMovementTime;
        yLocalPosStart = transform.localPosition.y + yOffset;
    }

    // Update is called once per frame
    void Update()
    {
        yTimer += Time.deltaTime;
        yCurrent = maxYmovement * Mathf.Sin(yTimer * omega);
        transform.localPosition = yLocalPosStart * Vector3.up + new Vector3(0, yCurrent, 0);
    }
}
