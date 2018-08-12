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

    private float turnVelocity;
    private float turnValue;

    private bool atSurface;
    private bool changingDepth;
    private bool beginCoroutine;
    
    public float moveAcceleration;
    public float turnAcceleration;
    public float moveSpeed;
    public float turnSpeed;
    public float minTurnSpeed;
    
    public float changeDepthSpeed;
    public float changeDepthTime;

    private void Awake()
    {
		playerMob = GetComponent<Mob>();
    }

    private void Start()
    {
        movementAxisName = "Vertical";
        turnAxisName = "Horizontal";

        turnValue = 0f;
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
		playerMob.setPercentForwardPower (movementAxisValue);
        turnAxisValue = Input.GetAxis(turnAxisName);
		playerMob.setPercentRotatePower (turnAxisValue);
    }

    private void FixedUpdate()
    {
		Move ();
        ToggleDepth();
    }

    private void Move()
    {

		if (atSurface)
        {
            //SurfaceAudio
        }
        else if (!atSurface)
        {
            //SubmergedAudio
        }

        //Debug.Log(rigidbody.velocity.magnitude);
    }

    private void Turn()
    {
        turnValue = 
            Mathf.SmoothDamp(turnValue, 
            turnAxisValue * Mathf.Clamp(turnSpeed * (rigidbody.velocity.magnitude / moveSpeed), minTurnSpeed, turnSpeed * (rigidbody.velocity.magnitude / moveSpeed)),
            ref turnVelocity, 1 / turnAcceleration);
        Quaternion turnRotation = Quaternion.Euler(0f, turnValue, 0f);
        rigidbody.MoveRotation(rigidbody.rotation * turnRotation);

        //Debug.Log(turn + "turn");
        //Debug.Log(turnAxisValue * turnSpeed * (rigidbody.velocity.magnitude / moveSpeed));
        //Debug.Log(turnRotation.eulerAngles);
        //Debug.Log(rigidbody.rotation.eulerAngles;  
    }
    private void ToggleDepth()
    {
		if (Input.GetButton("Fire2"))
        {
			gameObject.ChangeDepth ();
        }
        
    }

}

