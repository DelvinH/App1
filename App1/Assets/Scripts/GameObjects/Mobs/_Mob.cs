using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : DestructibleObject {

	//defaults are included.

	//"powered" movement
	//WIP: force falloff
	public bool canMoveForward = true;						//foward/backward
	public float movementForwardPower = 0.0f;					//current force
	public float movementForwardMaxPower = 1.0f;				//maximum force
	public float movementForwardMaxVelocity = 10.0f;				//maximum velocity

	public bool canMoveStrafe = false;						//left/right strafing
	public float movementStrafingPower = 0.0f;
	public float movementStrafingMaxPower = 1.0f;				
	public float movementStrafingMaxVelocity = 5.0f;

	public bool canMoveRotate = true;						//left/right turning
	public bool movementRotationRequiresForward = false;	//clamps max rotation power to forward speed
	public bool movementRotationDamped = false;				//damped rotation
	private float movementRotationCurrent = 0.0f;			//current rotation speed/value
	public float movementRotationDampTime = 1.0f;			//damping time for Mathf.SmoothDamp()
	public float movementRotationPower = 0.0f;					
	public float movementRotationMaxPower = 1.0f;				

	// Use this for initialization
	override public void Start ()
	{
		base.Start ();
	}

	// Update is called once per frame
	override public void Update ()
	{
		base.Update ();

	}

	override public void FixedUpdate(){
		handleMovement ();
		base.FixedUpdate ();
	}

	void setForwardPower(float forward){
		movementForwardPower = Mathf.Clamp (forward, -movementForwardMaxPower, movementForwardMaxPower);
	}

	void setStrafePower(float strafe){
		movementStrafingPower = Mathf.Clamp (strafe, -movementStrafingMaxPower, movementStrafingMaxPower);
	}

	void setRotatePower(float rotate){
		movementRotationPower = Mathf.Clamp (rotate, -movementRotationMaxPower, movementRotationMaxPower);
	}

	void handleMovement(){
		Vector3 forwardVector = transform.forward * movementForwardPower;
		Vector3 strafeVector = transform.right * movementStrafingPower;
		if(canMoveForward)
			AddForce (forwardVector);

			if (velocity.z >= movementForwardMaxVelocity) {
				velocity = velocity.normalized * movementForwardMaxVelocity;
			}

		if(canMoveStrafe)
			AddForce (strafeVector);
			
			if (velocity.x >= movementStrafingMaxVelocity) {
				velocity = velocity.normalized * movementStrafingMaxVelocity;
			}

		else {
			rigidbody.velocity = transform.forward * rigidbody.velocity.magnitude;		//prevents drifting if we can't strafe
		}

		float rotate = movementRotationPower;
		if(movementRotationRequiresForward) {
			rotate = Mathf.Clamp (movementRotationPower, -movementRotationMaxPower * Mathf.abs (velocity.z / movementForwardMaxVelocity), movementRotationMaxPower * Mathf.abs (velocity.z / movementForwardMaxVelocity));
		}
		if (movementRotationDamped) {
			movementRotationCurrent = Mathf.SmoothDamp (movementRotationCurrent, rotate, ref movementRotationCurrent, movementRotationDampTime, movementRotationMaxPower);
			rotate = movementRotationCurrent;
		}
		Quaternion turnRotation = Quaternion.Euler(0f, rotate, 0f);
		MoveRotation(rigidbody.rotation * turnRotation);
		//Full stop when not enough force to keep moving
		if (movementForwardPower < 0.01f && velocity.magnitude < 0.01f) {
			velocity = Vector3.zero;
		}

	}

}
