using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Script Parameters
    [Header("..")]
    [SerializeField, Min(0f)] float speed = 5f;
    [SerializeField, Min(0f)] float rotationSpeed = 90f;
    //[SerializeField, Min(0f)] float gravity = 5f; // can we jump? fall ?

    #endregion Script Parameters

    #region Fields

    // direction and rotation
    private int _moveForward = 0;
    private int _moveRotation = 0;

    #endregion Fields

    void Update()
    {
        transform.position += _moveForward * transform.forward * speed * Time.deltaTime; // += gravity * 
        transform.Rotate(Vector3.up, _moveRotation * rotationSpeed * Time.deltaTime);
    }

    public void SetMovementDirection(float vertical, float rotate)
    {
        _moveForward = (int)vertical;
        _moveRotation = (int)rotate;
    }
}
