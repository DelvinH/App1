﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{ 
    //Default values are included
    //public float positionDampSpeed;
    //public float angleDampSpeed;
    //public float maxDistanceInFront;
    public GameObject target;

    public float cameraDistance = 4.0f;
    public float cameraAngle = 20.0f;
    public float cameraHeight = 1.5f;

    private Transform cameraTransform;
    //private Vector3 moveVelocity;
    private Vector3 desiredPosition;
    private Quaternion desiredRotation;

    private void Awake()
    {
        cameraTransform = target.GetComponentInChildren<CameraTransformController>().GetTransform();
    }

    private void Update()
    {
        Move();
        //Debug.Log(moveVelocity.magnitude);
    }

    private void Move()
    {
        GetDesiredPosition();

        transform.position = desiredPosition;// Vector3.SmoothDamp(transform.position, desiredPosition, ref moveVelocity, 1 / positionDampSpeed);

        transform.rotation = desiredRotation;// Quaternion.RotateTowards(transform.rotation, desiredRotation, Mathf.Clamp(target.GetComponent<Rigidbody>().angularVelocity.magnitude, 1, target.GetComponent<Rigidbody>().angularVelocity.magnitude) * angleDampSpeed * Time.deltaTime);
    }

    private void GetDesiredPosition()
    {
        //desiredPosition = targetRigidbody.position + targetRigidbody.velocity.normalized * Mathf.Clamp(targetRigidbody.velocity.magnitude, 0f, maxDistanceInFront;

        desiredPosition = cameraTransform.position;
        desiredRotation = cameraTransform.rotation;
    }

    public float getCameraDistance()
    {
        return cameraDistance;
    }

    public float getCameraAngle()
    {
        return cameraAngle;
    }

    public float getCameraHeight()
    {
        return cameraHeight;
    }
}

