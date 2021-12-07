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
    private Vector3 localPosStart;

    // Start is called before the first frame update
    void Start()
    {
        omega = Mathf.PI * 2 / yMovementTime;
        localPosStart = transform.localPosition + yOffset * Vector3.up;
    }

    // Update is called once per frame
    void Update()
    {
        yTimer += Time.deltaTime;
        yCurrent = maxYmovement * Mathf.Sin(yTimer * omega);
        transform.localPosition = localPosStart + new Vector3(0, yCurrent, 0);
    }
}
