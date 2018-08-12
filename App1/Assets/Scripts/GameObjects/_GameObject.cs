using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OurGameObject : Rigidbody{

	public bool atSurface = false;

	// Use this for initialization
	virtual public void Start ()
	{

	}

	// Update is called once per frame
	virtual public void Update ()
	{
		RestrainMovement ();
	}

	virtual public void FixedUpdate(){
		RestrainMovement ();
	}

	private void RestrainMovement()//makes sure player is on game plane (y=0 for surface, y=-1 for submerged)
	{
		if (Mathf.Abs(transform.position.y + 1f) < Mathf.Abs(transform.position.y) && !changingDepth)//closer to -1
		{
			transform.position = new Vector3(transform.position.x, -1, transform.position.z);
		}
		else if (Mathf.Abs(transform.position.y + 1f) > Mathf.Abs(transform.position.y) && !changingDepth)//closer to 0
		{
			transform.position = new Vector3(transform.position.x, 0, transform.position.z);
		}
	} 
}
