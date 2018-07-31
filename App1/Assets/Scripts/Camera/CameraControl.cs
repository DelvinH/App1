using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{ 
    public float dampTime;
    public float maxDistanceInFront;

    private Camera camera;
    private Vector3 moveVelocity;
    private Vector3 desiredPosition;
    /*[HideInInspector*/
    public Transform target;
    private Rigidbody targetRigidbody;

    private void Awake()
    {
        camera = GetComponentInChildren<Camera>();
        targetRigidbody = target.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        getDesiredPosiiton();

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref moveVelocity, dampTime  / targetRigidbody.velocity.magnitude);
    }

    private void getDesiredPosiiton()
    {
        desiredPosition = target.position + targetRigidbody.velocity.normalized * Mathf.Min(targetRigidbody.velocity.magnitude, maxDistanceInFront);
    }
}

