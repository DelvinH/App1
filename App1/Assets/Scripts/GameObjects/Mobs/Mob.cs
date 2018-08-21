using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MobFactions {Neutral, Player, Hostile};

public class Mob : DestructibleObject {

    //Default values are included

    //Forward/Backward movement
    public bool canMoveForward = true;                  //can move forward/backward
    public bool movementForwardDamped = true;           //accelerates instantly if false
	public float movementForwardMaxPower = 1.0f;        //maximum power input value
	public float movementForwardMaxVelocity = 10.0f;    //maximum velocity
    public float timeToMaxVelocityForward = 1.0f;       //time to accelerate to max velocity

    private float movementForwardPower = 0.0f;	        //current power input value
    private float movementForwardValue = 0.0f;          //current forward velocity
    private float movementForwardReference;             //referenced used for Mathf.SmoothDamp

    //Right/Left movement
	public bool canMoveStrafe = false;					//can move left/right
    public bool movementStrafeDamped = true;            //accelerates instantly if false
    public float movementStrafeMaxPower = 1.0f;         //maximum power input value
	public float movementStrafeMaxVelocity = 2.0f;      //maximum velocity
    public float timeToMaxVelocityStrafe = 2.0f;        //time to accelerate to max velocity

    private float movementStrafePower = 0.0f;           //current power input value
    private float movementStrafeValue = 0.0f;           //current sideways velocity
    private float movementStrafeReference;              //reference used for Mathf.SmoothDamp

    //Right/Left turning
    public bool canMoveRotatation = true;					    //can turn left/right
    public bool movementRotationDamped = true;                  //accelerates instantly if false
	public bool movementRotationScalesWithVelocity = true;	    //max turning speed scales with percentage of max translational velocity
    public float movementRotationMinTurnVelocity = 15.0f;       //can turn at up to this velocity even if movementRotationScalesWithVelocity wouldn't allow it
	public float movementRotationMaxPower = 1.0f;               //maximum power input value
    public float movementRotationMaxVelocity = 90.0f;           //maximum velocity (degrees)
    public float timeToMaxVelocityRotation = 2.0f;              //time to accelerate to max velocity

    private float movementRotationPower = 0.0f;                 //current power input value
    private float movementRotationValue = 0.0f;                 //current angular velocity (degrees)
    private float movementRotationReference;                    //reference used for Mathf.SmoothDamp



	//Mob firing
	public Rigidbody projectileType;
	public Transform fireTransformLeft;
	public Transform fireTransformRight;
	public float fireSpeed;
	public float fireRate;
	public float minSpeedMultiplier;//varies torpedo speed

	private float timeSinceLastFireLeft;
	private float timeSinceLastFireRight;
	private bool leftIsReady;
	private bool rightIsReady;

	// Use this for initialization
	override public void Start ()
	{
		base.Start ();
        //InitializeFactions ();

		leftIsReady = false;
		rightIsReady = false;
	}

	// Update is called once per frame
	override public void Update ()
	{
		base.Update ();
		handleFiring ();
	}

	override public void FixedUpdate(){
		handleMovement ();
		base.FixedUpdate ();
	}

	/*public virtual void InitializeFactions(){
		factions.Clear ();
		factions.Add (MobFactions.Neutral);
	}*/



    /*Movement*/
	public void setForwardPower(float forward)
    {
		movementForwardPower = Mathf.Clamp (forward, -movementForwardMaxPower, movementForwardMaxPower);
	}

	public void setStrafePower(float strafe)
    {
		movementStrafePower = Mathf.Clamp (strafe, -movementStrafeMaxPower, movementStrafeMaxPower);
	}

	public void setRotationPower(float rotate)
    {
		movementRotationPower = Mathf.Clamp (rotate, -movementRotationMaxPower, movementRotationMaxPower);
	}

	public void setPercentForwardPower(float percent)
    {
		setForwardPower (movementForwardMaxPower * percent);
	}

	public void setPercentStrafePower(float percent)
    {
		setStrafePower (movementStrafeMaxPower * percent);
	}

	public void setPercentRotationPower(float percent)
    {
		setRotationPower (movementRotationMaxPower * percent);
	}

	void handleMovement(){
        handleForwardMovement();
        handleStrafeMovement();
        handleRotationMovement();

        handleStops();

        Debug.Log("movementForwardValue: " + movementForwardValue);
        Debug.Log("movementStrafeValue: " + movementStrafeValue);
        Debug.Log("movementRotationValue: " + movementRotationValue);

        /*v1
         * Vector3 forwardVector = transform.forward * movementForwardPower;
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
        */
    }

    private void handleForwardMovement()
    {
        movementForwardValue = movementForwardPower * movementForwardMaxVelocity;
        if (movementForwardDamped)
        {
            movementForwardValue = Mathf.SmoothDamp(movementForwardValue, movementForwardPower * movementForwardMaxVelocity, ref movementForwardReference, timeToMaxVelocityForward);
        }
        if (canMoveForward)
        {
            Vector3 movementForwardVector = rigidbody.position + transform.forward * movementForwardValue * Time.deltaTime;
            rigidbody.MovePosition(movementForwardVector);
        }
    }

    private void handleStrafeMovement()
    {
        movementStrafeValue = movementStrafePower * movementStrafeMaxVelocity;
        if (movementStrafeDamped)
        {
            movementStrafeValue = Mathf.SmoothDamp(movementStrafeValue, movementStrafePower * movementStrafeMaxVelocity, ref movementStrafeReference, timeToMaxVelocityStrafe);
        }
        if (canMoveStrafe)
        {
            Vector3 movementStrafeVector = rigidbody.position + transform.right * movementStrafeValue * Time.deltaTime;
            rigidbody.MovePosition(movementStrafeVector);
        }
    }

    private void handleRotationMovement()
    {
        movementRotationValue = movementRotationPower * movementRotationMaxVelocity;
        if (movementRotationScalesWithVelocity)
        {
            float currentVelocity = Mathf.Sqrt(Mathf.Pow(movementForwardValue, 2) + Mathf.Pow(movementStrafeValue, 2));//XZ plane speed only; add Y dimension if needed
            float maxVelocity = Mathf.Sqrt(Mathf.Pow(movementRotationMaxVelocity, 2) + Mathf.Pow(movementStrafeMaxVelocity, 2));

            float lowerRotationClamp = Mathf.Min(-movementRotationMaxVelocity * Mathf.Abs(currentVelocity / maxVelocity), -movementRotationMinTurnVelocity);//Sets clamp bounds to higher of velocity percentage or min turn velocity for low velocity turning
            float higherRotationClamp = Mathf.Max(movementRotationMaxVelocity * Mathf.Abs(currentVelocity / maxVelocity), movementRotationMinTurnVelocity);
            movementRotationValue = Mathf.Clamp(movementRotationValue, lowerRotationClamp, higherRotationClamp);
        }
        if (movementRotationDamped)
        {
            movementRotationValue = Mathf.SmoothDamp(movementRotationValue, movementRotationPower * movementStrafeMaxVelocity, ref movementRotationReference, timeToMaxVelocityRotation);
        }
        if (canMoveRotatation)
        {
            Quaternion movementTurnQuaternion = Quaternion.Euler(0.0f, movementRotationValue, 0.0f);//supports Y-axis turning only (XZ plane turning)
            rigidbody.MoveRotation(rigidbody.rotation * movementTurnQuaternion);
        }
    }

    private void handleStops()
    {
        if (movementForwardPower < 0.01f && movementForwardValue < 0.01f)
        {
            movementForwardValue = 0.0f;
        }
        if (movementStrafePower < 0.01f && movementStrafeValue < 0.01f)
        {
            movementStrafeValue = 0.0f;
        }
        if (movementRotationPower < 0.01f && movementRotationValue < 0.01f)
        {
            movementRotationValue = 0.0f;
        }
    }







    /*Firing*/
    private void handleFiring(){
		leftIsReady = Time.time - timeSinceLastFireLeft > 1 / fireRate;
		rightIsReady = Time.time - timeSinceLastFireRight > 1 / fireRate;
	}

	public void tryFire(){
		if (leftIsReady && rightIsReady) {
			float rand = Random.value - 0.5f;
			if (rand <= 0) {
				doFireLeft ();
			} else if (rand > 0) {
				doFireRight ();
			}
			//Debug.Log(rand + "rand");
		} else if (leftIsReady) {
			doFireLeft ();
		} else if (rightIsReady) {
			doFireRight ();
		}

	}

	public void doFireLeft()
	{
		//Debug.Log("firedleft");
		Rigidbody projectileLeft = Instantiate(projectileType, fireTransformLeft.position, transform.rotation) as Rigidbody;
		projectileLeft.velocity = fireTransformLeft.forward * fireSpeed * Random.Range(minSpeedMultiplier, 1);
		leftIsReady = false;
		timeSinceLastFireLeft = Time.time;
		//Audio
	}

	public void doFireRight()
	{
		//Debug.Log("firedright");
		Rigidbody projectileRight = Instantiate(projectileType, fireTransformRight.position, transform.rotation) as Rigidbody;
		projectileRight.velocity = fireTransformRight.forward * fireSpeed * Random.Range(minSpeedMultiplier, 1);
		rightIsReady = false;
		timeSinceLastFireRight = Time.time;
		//Audio
	}

    public float currentSpeedPercentage()//for camera zoom when moving fast effect
    {
        return movementForwardValue / movementForwardMaxVelocity;
    }
}
