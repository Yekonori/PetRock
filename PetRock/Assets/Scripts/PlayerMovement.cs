using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // direction and rotation
    int moveForward = 0;
    int moveRotation = 0;
    [Range(0f, 0.5f)] float deadZone = 0.4f;

    [Header("..")]
    [SerializeField, Min(0f)] float speed = 5f;
    [SerializeField, Min(0f)] float rotationSpeed = 90f;
    //[SerializeField, Min(0f)] float gravity = 5f; // can we jump? fall ?

    // Start is called before the first frame update
    //void Start()
    //{
        
    //}

    // Update is called once per frame
    void Update()
    {
        transform.position += moveForward * transform.forward * speed * Time.deltaTime; // += gravity * 
        transform.Rotate(Vector3.up, moveRotation * rotationSpeed * Time.deltaTime);
    }

    public void SetMovementDirection(float vertical, float rotate)
    {
        moveForward = Mathf.Abs(vertical) > deadZone ? (int)Mathf.Sign(vertical) : 0;
        moveRotation = Mathf.Abs(rotate) > deadZone ? (int)Mathf.Sign(rotate) : 0; ;
    }
}
