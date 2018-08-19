using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobAI : MonoBehaviour {

	private Mob OurMob;

	public LayerMask targetLayerMask;

	//public float basicDetectionRange = 30.0f;		//will detect things within this range
	//public float basicLockRange = 80.0f;			//will keep locks to already detected things within this range
	//public bool onlyLockSameDepth = true;			//will lock only things on surface if at surface or below if below etc
	//public IList<GameObject> targets = new List<GameObject>();

	// Use this for initialization
	virtual public void Start () {
		OurMob = gameObject.GetComponent<Mob> ();
		if (!OurMob) {
			Debug.LogWarning ("MobAI added to something that does not have a mob component", this);
			Destroy (this);
		}
	}

	// Update is called once per frame
	virtual public void Update ()
	{
		//CheckTarget ();
	}

	/*
	public void updateTargets(){

	}

	virtual public void pruneTargets(){
		foreach (GameObject theObject in targets) {
			if (!canKeepTarget (theObject)) {
				targets.Remove(theObject);
			}
		}
	}

	//Whether this can acquire the object as a target
	virtual public bool canAcquireTarget(GameObject theObject){
		if (!canKeepTarget (theObject)) {
			return false;
		}
		Mob PotentialEnemy = theObject.GetComponent<Mob>();
		//If they're not a mob, ignore.
		if (!PotentialEnemy) {
			return false;
		}
		//If we don't want their layer, ignore.
		if (targetLayerMask != (targetLayerMask | (1 << theObject.layer))) {
			return false;
		}
		//distance
		return true;
	}

	virtual public bool factionCheckFriendly(Mob other){
		if (OurMob.factions.Intersect(PotentialEnemy.factions).Count()) {
			return true;
		}
		return false;
	}

	//Whether this can keep targeting the object
	virtual public bool canKeepTarget(GameObject theObject){
		if (!canSee (theObject)) {
			return false;
		}
		//distance
		return true;
	}

	//Whether this can see the object at all
	virtual public bool canSee(GameObject theObject){
		//distance
		return true;
	}*/

}
