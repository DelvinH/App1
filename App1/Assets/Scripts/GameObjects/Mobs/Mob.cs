using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MobFactions {Neutral, Player, Hostile};

public class Mob : DestructibleObject {

	//defaults are included.

	//targeting things
	public IList<MobFactions> factions = new List<MobFactions>();

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
		InitializeFactions ();
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

	public virtual void InitializeFactions(){
		factions.Clear ();
		factions.Add (MobFactions.Neutral);
	}

	public void setForwardPower(float forward){
		movementForwardPower = Mathf.Clamp (forward, -movementForwardMaxPower, movementForwardMaxPower);
	}

	public void setStrafePower(float strafe){
		movementStrafingPower = Mathf.Clamp (strafe, -movementStrafingMaxPower, movementStrafingMaxPower);
	}

	public void setRotatePower(float rotate){
		movementRotationPower = Mathf.Clamp (rotate, -movementRotationMaxPower, movementRotationMaxPower);
	}

	public void setPercentForwardPower(float percent){
		setForwardPower (movementForwardMaxPower * percent);
	}

	public void setPercentStrafePower(float percent){
		setStrafePower (movementStrafingMaxPower * percent);
	}

	public void setPercentRotatePower(float percent){
		setRotatePower (movementRotationMaxPower * percent);
	}

	void handleMovement(){
		Vector3 forwardVector = transform.forward * movementForwardPower;
		Vector3 strafeVector = transform.right * movementStrafingPower;
		if(canMoveForward)
			rigidbody.AddForce (forwardVector);

			if (rigidbody.velocity.z >= movementForwardMaxVelocity) {
				rigidbody.velocity = rigidbody.velocity.normalized * movementForwardMaxVelocity;
			}

		if(canMoveStrafe)
			rigidbody.AddForce (strafeVector);
			
			if (rigidbody.velocity.x >= movementStrafingMaxVelocity) {
				rigidbody.velocity = rigidbody.velocity.normalized * movementStrafingMaxVelocity;
			}

		else {
			rigidbody.velocity = transform.forward * rigidbody.velocity.magnitude;		//prevents drifting if we can't strafe
		}

		float rotate = movementRotationPower;
		if(movementRotationRequiresForward) {
			rotate = Mathf.Clamp (movementRotationPower, -movementRotationMaxPower * Mathf.Abs (rigidbody.velocity.z / movementForwardMaxVelocity), movementRotationMaxPower * Mathf.Abs (rigidbody.velocity.z / movementForwardMaxVelocity));
		}
		if (movementRotationDamped) {
			movementRotationCurrent = Mathf.SmoothDamp (movementRotationCurrent, rotate, ref movementRotationCurrent, movementRotationDampTime, movementRotationMaxPower);
			rotate = movementRotationCurrent;
		}
		Quaternion turnRotation = Quaternion.Euler(0f, rotate, 0f);
		rigidbody.MoveRotation(rigidbody.rotation * turnRotation);
		//Full stop when not enough force to keep moving
		if (movementForwardPower < 0.01f && rigidbody.velocity.magnitude < 0.01f) {
			rigidbody.velocity = Vector3.zero;
		}

	}

}
