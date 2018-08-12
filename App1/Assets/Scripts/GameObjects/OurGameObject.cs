using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OurGameObject : MonoBehaviour{

	protected Rigidbody rigidbody;
	public bool atSurface = true;
	private bool changingDepth = false;
	public float changeDepthSpeed = 0.5f;
	public float changeDepthTime = 2f;

	// Use this for initialization
	virtual public void Start ()
	{
		rigidbody = gameObject.GetComponent<Rigidbody> ();
		if (!rigidbody) {
			Debug.LogWarning ("Game Object without rigidbody", this);
		}
	}

	// Update is called once per frame
	virtual public void Update ()
	{
		RestrainMovement ();
	}

	virtual public void FixedUpdate(){
		RestrainMovement ();
	}

	public bool ToggleDepth()
	{	
		if (changingDepth) {
			return false;
		}
		bool target = !atSurface;
		IEnumerator coroutine = ChangeDepth (target);
		return true;
	}

	private IEnumerator ChangeDepth(bool atSurface)
	{
		Vector3 movement;
		float timeElapsed = Time.time;
		changingDepth = true;

		if (atSurface)
		{
			this.atSurface = false;//submarine leaves surface plane at beginning of dive
			//Audio
		}

		while (Time.time - timeElapsed < changeDepthTime)
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

	public bool getChangingDepth()
	{
		return changingDepth;
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
