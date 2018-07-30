using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rigidbody;
    private string movementAxisName;
    private string turnAxisName;
    private float movementAxisValue;
    private float turnAxisValue;

    public float moveAcceleration;
    public float turnAcceleration;
    public float maxMoveSpeed;
    public float maxTurnSpeed;
    


    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        movementAxisName = "Vertical";
        turnAxisName = "Horizontal";
    }

    /*private void OnDisable()
    {
        rigidbody.isKinematic = false;

        movementAxisValue = 0f;
        turnAxisValue = 0f;
    }

    private void OnEnable()
    {
        rigidbody.isKinematic = true;
    }*/

    private void Update()
    {
        movementAxisValue = Input.GetAxis(movementAxisName);
        turnAxisValue = Input.GetAxis(turnAxisName);
    }

    private void FixedUpdate()
    {
        Move();
        Turn();
    }

    private void Move()
    {
        Vector3 movement = transform.forward * movementAxisValue * moveAcceleration;

        rigidbody.AddForce(movement);


        if (rigidbody.velocity.magnitude >= maxMoveSpeed)
            rigidbody.velocity = rigidbody.velocity.normalized * maxMoveSpeed;

        /*if(movementAxisValue < 0.1f*/
            rigidbody.velocity = transform.forward * rigidbody.velocity.magnitude;
        
    }

    private void Turn()
    {
        Vector3 turn = transform.up * turnAxisValue * turnAcceleration * (rigidbody.velocity.magnitude / maxMoveSpeed);

        rigidbody.AddTorque(turn);

        if (rigidbody.angularVelocity.magnitude >= maxTurnSpeed * (rigidbody.velocity.magnitude / maxMoveSpeed))
            rigidbody.angularVelocity = rigidbody.angularVelocity.normalized * maxTurnSpeed * (rigidbody.velocity.magnitude / maxMoveSpeed);


        if (Mathf.Abs(turnAxisValue) < 0.1f)
            rigidbody.angularVelocity = Vector3.zero;
    }
}

