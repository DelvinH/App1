using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobAI : MonoBehaviour {

	private Mob OurMob;

	// Use this for initialization
	void Start () {
		OurMob = gameObject.GetComponent<Mob> ();
		if (!OurMob) {
			Debug.LogWarning ("MobAI added to something that does not have a mob component", this);
			Destroy (this);
		}
	}

	// Update is called once per frame
	void Update ()
	{

	}

}
