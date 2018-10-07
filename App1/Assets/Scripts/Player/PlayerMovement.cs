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

    private void Awake()
    {
		playerMob = gameObject.GetComponent<Mob>();
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

    private void Update()//Add audio
    {
        movementAxisValue = Input.GetAxis(movementAxisName);
        turnAxisValue = Input.GetAxis(turnAxisName);

        playerMob.setPercentForwardPower(movementAxisValue);
        playerMob.setPercentRotationPower(turnAxisValue);
        
        
    }

    private void FixedUpdate()
    {
        ChangeDepth();
    }

    private void ChangeDepth()
    {
		if (Input.GetButton("Fire2"))
        {
			playerMob.ChangeDepth();
        }
    }

}

