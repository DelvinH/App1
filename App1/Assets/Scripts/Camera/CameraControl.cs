using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{ 
    public float dampTime;

    private Camera camera;
    private Vector3 moveVelocity;
    private Vector3 desiredPosition;
    /*[HideInInspector*/
    public Transform target;

    private void Awake()
    {
        camera = GetComponentInChildren<Camera>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        getDesiredPosiiton();

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref moveVelocity, dampTime);
    }

    private void getDesiredPosiiton()
    {
        desiredPosition = target.position;
    }
}

