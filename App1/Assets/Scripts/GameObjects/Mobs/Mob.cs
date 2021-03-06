﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MobFactions {Neutral, Player, Hostile};

public enum Cardinal {North, South, East, West};

public class Mob : DestructibleObject {

    //Default values are included

    //Forward/Backward movement
    public bool canMoveForward = true;                  //can move forward/backward
    public bool movementForwardDamped = true;           //accelerates instantly if false
	public float movementForwardMaxPower = 1.0f;        //maximum power input value
	public float movementForwardMaxVelocity = 10.0f;    //maximum velocity
    public float timeToMaxVelocityForward = 1.0f;       //time to accelerate to max velocity

    protected float movementForwardPower = 0.0f;	    //current power input value
    protected float movementForwardValue = 0.0f;        //current forward velocity
    protected float movementForwardReference = 0.0f;    //referenced used for Mathf.SmoothDamp

    //Right/Left movement
	public bool canMoveStrafe = false;					//can move left/right
    public bool movementStrafeDamped = true;            //accelerates instantly if false
    public float movementStrafeMaxPower = 1.0f;         //maximum power input value
	public float movementStrafeMaxVelocity = 2.0f;      //maximum velocity
    public float timeToMaxVelocityStrafe = 2.0f;        //time to accelerate to max velocity

    protected float movementStrafePower = 0.0f;         //current power input value
    protected float movementStrafeValue = 0.0f;         //current sideways velocity
    protected float movementStrafeReference = 0.0f;     //reference used for Mathf.SmoothDamp

    //Right/Left turning
    public bool canMoveRotatation = true;					    //can turn left/right
    public bool movementRotationDamped = true;                  //accelerates instantly if false
	public float movementRotationMaxPower = 1.0f;               //maximum power input value
    public float movementRotationMaxVelocity = 90.0f;           //maximum velocity (degrees)
    public float timeToMaxVelocityRotation = 2.0f;              //time to accelerate to max velocity

    protected float movementRotationPower = 0.0f;               //current power input value
    protected float movementRotationValue = 0.0f;               //current angular velocity (degrees)
    protected float movementRotationReference = 0.0f;           //reference used for Mathf.SmoothDamp

    //Changing depth movement
    public float changeDepthTime = 1.0f;
    public float depthChanged = 1.0f;

    protected bool atSurface = true;
    protected bool changingDepth = false;
    protected float changeDepthSpeed = 1.0f;//This equals depthChanged / changeDepthTime

    //Mob firing
    public Transform fireTransform;
    public Rigidbody projectileType;
    public float burstNumber;//Number of shots fired one after another
    public float spreadNumber;//Number of shots fired at the same time
    public float burstFireRate;
	public float fireRate;
    public float accuracyVariation;

    protected float timeSinceLastFire;
	



	// Use this for initialization
	override public void Start ()
	{
		base.Start ();
        //InitializeFactions ();

	}

	// Update is called once per frame
	override public void Update ()
	{
		base.Update ();
	}

	override public void FixedUpdate()
    {
		handleMovement ();
		RestrainMovement ();
		base.FixedUpdate ();
        RestrainMovement();
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

	virtual protected void handleMovement()//Status: Working
    {
        handleForwardMovement();
        handleStrafeMovement();
        handleRotationMovement();

        handleStops();

        //Debug.Log("movementForwardValue: " + movementForwardValue);
        //Debug.Log("movementStrafeValue: " + movementStrafeValue);
        //Debug.Log("movementRotationValue: " + movementRotationValue);
    }

    virtual protected void handleForwardMovement()//Status: Working
    {
        if (movementForwardDamped)
        {
            movementForwardValue = Mathf.SmoothDamp(movementForwardValue, movementForwardPower * movementForwardMaxVelocity, ref movementForwardReference, timeToMaxVelocityForward);
        }
        else
        {
            movementForwardValue = movementForwardPower * movementForwardMaxVelocity;
        }
        if (canMoveForward)
        {
            Vector3 movementForwardVector = rigidbody.position + transform.forward * movementForwardValue * Time.deltaTime;
            rigidbody.MovePosition(movementForwardVector);
        }
    }

    virtual protected void handleStrafeMovement()//Status: Working
    {
        if (movementStrafeDamped)
        {
            movementStrafeValue = Mathf.SmoothDamp(movementStrafeValue, movementStrafePower * movementStrafeMaxVelocity, ref movementStrafeReference, timeToMaxVelocityStrafe);
        }
        else
        {
            movementStrafeValue = movementStrafePower * movementStrafeMaxVelocity;
        }
        if (canMoveStrafe)
        {
            Vector3 movementStrafeVector = rigidbody.position + transform.right * movementStrafeValue * Time.deltaTime;
            rigidbody.MovePosition(movementStrafeVector);
        }
    }

    virtual protected void handleRotationMovement()//Status: Working
    {
        if (movementRotationDamped)
        {
            movementRotationValue = Mathf.SmoothDamp(movementRotationValue, movementRotationPower * movementRotationMaxVelocity, ref movementRotationReference, timeToMaxVelocityRotation);
        }
        else
        {
            movementRotationValue = movementRotationPower * movementRotationMaxVelocity;
        }
        
        if (canMoveRotatation)
        {
            Quaternion movementTurnQuaternion = Quaternion.Euler(0.0f, movementRotationValue * Time.deltaTime, 0.0f);
            rigidbody.MoveRotation(rigidbody.rotation * movementTurnQuaternion);
        }
    }

    private void handleStops()//Status: Working
    {
        if (Mathf.Abs(movementForwardPower) < 0.01f && Mathf.Abs(movementForwardValue) < 0.01f)
        {
            movementForwardValue = 0.0f;
        }
        if (Mathf.Abs(movementStrafePower) < 0.01f && Mathf.Abs(movementStrafeValue) < 0.01f)
        {
            movementStrafeValue = 0.0f;
        }
        if (Mathf.Abs(movementRotationPower) < 0.01f && Mathf.Abs(movementRotationValue) < 0.01f)
        {
            movementRotationValue = 0.0f;
        }
    }

    public bool ChangeDepth()//Status: Working
    {
        if (changingDepth)
        {
            return false;
        }
        StartCoroutine(ChangeDepthCoroutine(atSurface));
        return true;
    }

    private IEnumerator ChangeDepthCoroutine(bool atSurface)//Status: Working
    {
        Vector3 movement;
        float timeAtStart = Time.time;
        changingDepth = true;
        changeDepthSpeed = depthChanged / changeDepthTime;

        if (atSurface)
        {
            this.atSurface = false;//submarine leaves surface plane at beginning of dive
            //Audio
        }

        while (Time.time - timeAtStart < changeDepthTime)
        {
            if (atSurface)//if loop must be in while loop for proper Time.deltaTime values
                movement = transform.up * changeDepthSpeed * Time.deltaTime * -1f;//negative makes submarine go down
            else
                movement = transform.up * changeDepthSpeed * Time.deltaTime;//positive makes submarine go up
            rigidbody.MovePosition(rigidbody.position + movement);
            yield return null;
        }

        if (!atSurface)
        {
            this.atSurface = true;//submarine enters surface plane at end of surface
            //Audio
        }

        changingDepth = false;
    }

    public bool getChangingDepth()//Status: Working
    {
        return changingDepth;
    }

    private void RestrainMovement()//Status: Working
    {//makes sure mob is on game plane (y=0 for surface, y=depthChanged for submerged)
        if (Mathf.Abs(transform.position.y + depthChanged) < Mathf.Abs(transform.position.y) && !changingDepth)//closer to lower depth
        {
            transform.position = new Vector3(transform.position.x, -depthChanged, transform.position.z);
        }
        else if (Mathf.Abs(transform.position.y + depthChanged) > Mathf.Abs(transform.position.y) && !changingDepth)//closer to 0
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
    }








    /*Firing*/
    public void handleFiring()
    {
        if (fireRate < burstFireRate)
        {
            throw new System.Exception("Fire rate must be greater than burst rate!");
        }
	    if (Time.time > timeSinceLastFire)
        {
            StartCoroutine(handleFiringCoroutine());
        }
	}

    private IEnumerator handleFiringCoroutine()
    {

        timeSinceLastFire = Time.time + fireRate;//timeSinceLastFire needs to be updated several times to prevent multiple bursts from firing simultaneously
        Vector3 rotation = fireTransform.TransformDirection(fireTransform.forward);
        if (!changingDepth)
        {
            for (int i = 0; i < spreadNumber; i++)
            {
                rotation = fireTransform.rotation.eulerAngles;
                rotation = new Vector3(rotation.x, rotation.y + Random.Range(accuracyVariation, -accuracyVariation), rotation.z);
                Instantiate(projectileType, fireTransform.position, Quaternion.Euler(rotation));
            }   
        }
            
            
        
        
        for (int i = 0; i < burstNumber - 1; i++)
        {
            yield return new WaitForSeconds(burstFireRate);
            if (!changingDepth)
            {
                for (int j = 0; j < spreadNumber; j++)
                {
                    rotation = fireTransform.rotation.eulerAngles;
                    rotation = new Vector3(rotation.x, rotation.y + Random.Range(accuracyVariation, -accuracyVariation), rotation.z);
                    Instantiate(projectileType, fireTransform.position, Quaternion.Euler(rotation));
                }
            }
            timeSinceLastFire = Time.time + fireRate;
        }
        timeSinceLastFire = Time.time + fireRate;
        
    }









	//AI helpers
	public float distanceToPlayer()
    {		//straight distance to player ignoring all obstacles
		Vector3 vector = getVectorToPlayer();
        float distance = vector.magnitude;

		return distance;
	}

	public float angleToPlayer()
    {          //returns bearing to player
		Vector3 vector = getVectorToPlayer();
        float theta = Mathf.Atan2(vector.z, vector.x);
        
        if (theta < 0)
            theta += 2 * Mathf.PI;
        
        return theta;
	}

	public Vector3 relativePlayerSpeed()
    {   //returns speed of player relative to mob
        Mob playerMob = Globals.ThePlayer.GetComponent<Mob>();
        Debug.Log(transform.TransformVector(new Vector3 (1, 0, 0)));
        return transform.TransformVector(playerMob.getVelocity()) - transform.TransformVector(getVelocity());
	}

	public float relativePlayerAngle()
    {  //returns angle of velocity of player relative to mob
		float their_angle = Mathf.Ceil (angleToPlayer());
		float our_angle = gameObject.transform.eulerAngles.y;
		return their_angle - our_angle;

	}

	

	public Vector3 getVectorToPlayer()
    {
		GameObject player = Globals.ThePlayer.gameObject;
		return player.transform.position - gameObject.transform.position;
	}








    //Return functions
    public Vector3 getVelocity()
    {
        return new Vector3(movementForwardValue, 0f, movementStrafeValue);
    }
}
