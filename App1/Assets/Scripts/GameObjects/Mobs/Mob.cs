using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MobFactions {Neutral, Player, Hostile};

public class Mob : DestructibleObject {

    //Default values are included
	protected Rigidbody rigidbody;

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

    //Mob firing
    public Transform fireTransform;
    public Rigidbody projectileType;
    public float burstNumber;
    public float burstFireRate;
	public float fireRate;
    public float accuracyVariation;
    

    private float timeSinceLastFire;
	

	//Submerging
	public bool atSurface = true;
	private bool changingDepth = false;
	public float changeDepthSpeed = 0.5f;
	public float changeDepthTime = 2f;


	// Use this for initialization
	override public void Start ()
	{
		base.Start ();
		rigidbody = gameObject.GetComponent<Rigidbody> ();
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
            Vector3 movementForwardVector = transform.forward * movementForwardValue * Time.deltaTime;
            rigidbody.MovePosition(rigidbody.position + movementForwardVector);
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
    {//makes sure mob is on game plane (y=0 for surface, y=-1 for submerged)
        if (Mathf.Abs(transform.position.y + 1f) < Mathf.Abs(transform.position.y) && !changingDepth)//closer to -1
        {
            transform.position = new Vector3(transform.position.x, -1, transform.position.z);
        }
        else if (Mathf.Abs(transform.position.y + 1f) > Mathf.Abs(transform.position.y) && !changingDepth)//closer to 0
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
    }


    /*Firing*/
    public void handleFiring()
    {
	    if (Time.time > timeSinceLastFire)
        {
            StartCoroutine(handleFiringCoroutine());
        }
	}

    private IEnumerator handleFiringCoroutine()
    {
        Vector3 rotation = fireTransform.TransformDirection(fireTransform.forward);
        rotation = fireTransform.rotation.eulerAngles;
        rotation = new Vector3(rotation.x, rotation.y + Random.Range(accuracyVariation, -accuracyVariation), rotation.z);
        Instantiate(projectileType, fireTransform.position, Quaternion.Euler(rotation));
        for (int i = 0; i < burstNumber - 1; i++)
        {
            yield return new WaitForSeconds(burstFireRate);

            rotation = fireTransform.TransformDirection(fireTransform.forward);
            rotation = fireTransform.rotation.eulerAngles;
            rotation = new Vector3(rotation.x, rotation.y + Random.Range(accuracyVariation, -accuracyVariation), rotation.z);
            Instantiate(projectileType, fireTransform.position, Quaternion.Euler(rotation));
        }
        timeSinceLastFire = Time.time + fireRate;
    }



	//AI helpers
	public float distanceToPlayer(){		//straight distance to player ignoring all obstacles
		Vector3 diff = getVectorToPlayer();
		return diff.magnitude;
	}

	public float angleToPlayer(){
		return getTransformToPlayer ().eulerAngles.y;
	}

	public float playerCurrentSpeed(){
		return Globals.ThePlayer.movementForwardValue;
	}

	public Transform getTransformToPlayer(){
		GameObject theirs = Globals.ThePlayer.gameObject;
		return theirs.Transform - gameObject.Transform;
	}

	public Vector3 getVectorToPlayer(){
		return getTransformToPlayer ().position;
	}

	public string directionToPlayer(){
		float angle = angleToPlayer ();
		string retval = "ERROR";
		if (315 < angle < 45) {			//needs to be made into an enum later
			retval = "FORWARD";
		}
		if (45 < angle < 135) {
			retval = "RIGHT";
		}
		if (135 < angle < 225) {
			retval = "BEHIND";
		}
		if (225 < angle < 315) {
			retval = "LEFT";
		}
		return "ERROR";
	}


}
