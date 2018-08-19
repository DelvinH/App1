using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMob : Mob {

	// Use this for initialization
	override public void Start () {
		if (Globals.ThePlayer != this) {
			Destroy (Globals.ThePlayer);
		}
		Globals.ThePlayer = this;
		base.Start ();
	}
	
	// Update is called once per frame
	override public void Update () {
		base.Update ();
	}
}
