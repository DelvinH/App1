using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Mob playerMob;
    private string movementAxisName;
    private string turnAxisName;
    private float movementAxisValue;
    private float turnAxisValue;

    //private float turnVelocity;
    //private float turnValue;

    //public float moveAcceleration;
    //public float turnAcceleration;
    //public float moveSpeed;
    //public float turnSpeed;
    //public float minTurnSpeed;

    private void Awake()
    {
		playerMob = gameObject.GetComponent<Mob>();
    }

    private void Start()
    {
        movementAxisName = "Vertical";
        turnAxisName = "Horizontal";

        //turnValue = 0f;
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

        playerMob.setPercentForwardPower(movementAxisValue);
        playerMob.setPercentRotatePower(turnAxisValue);
    }

    private void FixedUpdate()
    {
		Move ();
        ToggleDepth();
    }

    private void Move()
    {

		if (playerMob.atSurface)
        {
            //SurfaceAudio
        }
		else if (!playerMob.atSurface)
        {
            //SubmergedAudio
        }

        //Debug.Log(rigidbody.velocity.magnitude);
    }

    private void Turn()
    {
        /*turnValue = 
            Mathf.SmoothDamp(turnValue, 
			turnAxisValue * Mathf.Clamp(turnSpeed * (playerMob.velocity.magnitude / moveSpeed), minTurnSpeed, turnSpeed * (playerMob.velocity.magnitude / moveSpeed)),
            ref turnVelocity, 1 / turnAcceleration);
        Quaternion turnRotation = Quaternion.Euler(0f, turnValue, 0f);
        rigidbody.MoveRotation(playerMob.rotation * turnRotation);*/

        //Debug.Log(turn + "turn");
        //Debug.Log(turnAxisValue * turnSpeed * (rigidbody.velocity.magnitude / moveSpeed));
        //Debug.Log(turnRotation.eulerAngles);
        //Debug.Log(rigidbody.rotation.eulerAngles;  
    }
    private void ToggleDepth()
    {
		if (Input.GetButton("Fire2"))
        {
			playerMob.ToggleDepth ();
        }
        
    }

}

