using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobAI : MonoBehaviour {

	const int AI_OFF = 0;
	const int AI_FOLLOW = 1;
	//const int AI_PATROL = 2;

	private Mob OurMob;

	public LayerMask targetLayerMask;

	public bool onlyTargetSameDepth = true;			//only go after things in the same depth

	public int AIState = AI_OFF;

	public GameObject target;

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
		CheckTarget ();
		HandleAI ();
	}

	virtual public void CheckTarget(){
		target = Globals.ThePlayer;
		AIState = AI_FOLLOW;
	}

	virtual public void HandleAI(){
		switch (AIState) {
		case AI_OFF:
			return;
		case AI_FOLLOW:
			//follow 
			return;
		}
	}

	virtual public void GoTo(){

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
