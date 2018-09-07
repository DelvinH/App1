using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMob : Mob {
    public bool movementRotationScalesWithVelocity = true;	    //max turning speed scales with percentage of max translational velocity
    //public float movementRotationMinTurnVelocity = 15.0f;       //can turn at up to this velocity even if movementRotationScalesWithVelocity wouldn't allow it
    public float movementBackwardsMaxVelocity = 4.0f;           //max velocity backwards


    override public void Start () {
		if (Globals.ThePlayer != this) {
			Destroy (Globals.ThePlayer);
		}
		Globals.ThePlayer = this;
		base.Start ();
	}
	
	override public void Update () {
		base.Update ();
	}

    override public void FixedUpdate()
    {
        base.FixedUpdate();
    }

    override protected void handleForwardMovement()
    {
        if (movementForwardDamped)
        {
            movementForwardValue = Mathf.SmoothDamp(movementForwardValue, movementForwardPower * movementForwardMaxVelocity, ref movementForwardReference, timeToMaxVelocityForward);
        }
        else
        {
            movementForwardValue = movementForwardPower * movementForwardMaxVelocity;
        }
        if (movementForwardValue < -movementBackwardsMaxVelocity)
        {
            movementForwardValue = -movementBackwardsMaxVelocity;
        }
        if (canMoveForward)
        {
            Vector3 movementForwardVector = transform.forward * movementForwardValue * Time.deltaTime;
            rigidbody.MovePosition(rigidbody.position + movementForwardVector);
        }
    }

    override protected void handleRotationMovement()
    {
        if (movementRotationDamped)
        {
            movementRotationValue = Mathf.SmoothDamp(movementRotationValue, movementRotationPower * movementRotationMaxVelocity, ref movementRotationReference, timeToMaxVelocityRotation);
        }
        else
        {
            movementRotationValue = movementRotationPower * movementRotationMaxVelocity;
        }
        if (movementRotationScalesWithVelocity)
        {
            float currentVelocity = Mathf.Sqrt(Mathf.Pow(movementForwardValue, 2) + Mathf.Pow(movementStrafeValue, 2));//XZ plane speed only; add Y dimension if needed
            float maxVelocity = Mathf.Sqrt(Mathf.Pow(movementForwardMaxVelocity, 2) + Mathf.Pow(movementStrafeMaxVelocity, 2));

            float lowerRotationClamp = /*Mathf.Min(*/-movementRotationMaxVelocity * Mathf.Abs(currentVelocity / maxVelocity)/*, -movementRotationMinTurnVelocity)*/;//Sets clamp bounds to higher of velocity percentage or min turn velocity for low velocity turning
            float higherRotationClamp = /*Mathf.Max(*/movementRotationMaxVelocity * Mathf.Abs(currentVelocity / maxVelocity)/*, movementRotationMinTurnVelocity)*/;
            movementRotationValue = Mathf.Clamp(movementRotationValue, lowerRotationClamp, higherRotationClamp);
        }
        if (canMoveRotatation)
        {
            Quaternion movementTurnQuaternion;
            if (movementForwardValue >= 0.0f) //supports Y-axis turning only (XZ plane turning)
            {
                movementTurnQuaternion = Quaternion.Euler(0.0f, movementRotationValue * Time.deltaTime, 0.0f);
            }
            else
            {
                movementTurnQuaternion = Quaternion.Euler(0.0f, movementRotationValue * Time.deltaTime * -1, 0.0f);//Turning is reversed when going backwards
            }

            rigidbody.MoveRotation(rigidbody.rotation * movementTurnQuaternion);
        }
    }
}
